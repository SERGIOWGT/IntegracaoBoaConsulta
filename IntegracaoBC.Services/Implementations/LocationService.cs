using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _iLocationRepository;
        private readonly IConsultorioRepository _iConsultorioRepository;
        public LocationService(ILocationRepository iLocationRepository, IConsultorioRepository iConsultorioRepository)
        {
            _iLocationRepository = iLocationRepository;
            _iConsultorioRepository = iConsultorioRepository;
        }
        
        public async Task<IEnumerable<string>> Sync()
        {
            List<string> _erros = new();

            List<ConsultorioResponse> _consultorios = (List<ConsultorioResponse>)await _iConsultorioRepository.GetAll();
            if (_consultorios.Count == 0)
            {
                _erros.Add("Não há consultórios cadastrados na base da 021 Dental");
                return _erros;
            }
            List<LocationResponse> _locations = (List<LocationResponse>)await _iLocationRepository.GetAll();

            await CompatilizaConsultorios(_consultorios, _locations, _erros);


            return _erros;
        }


        private void DesmembraEndereco(string endereco, ref string rua, ref string numero)
        {
            rua = "";
            numero = "0";

            if (!string.IsNullOrEmpty(endereco))
            {
                string[] _listaEndereco = endereco.Split(new Char[] { ',', ' ' });
                if (_listaEndereco.Length == 1)
                {
                    endereco = _listaEndereco[0].Trim().ToUpper();
                }
                else if (_listaEndereco.Length > 1)
                {
                    var _numeroNomes = _listaEndereco.Length;
                    if (long.TryParse(_listaEndereco[^1].Trim(), out _))
                    {
                        numero = _listaEndereco[^1].Trim();
                        _numeroNomes = _listaEndereco.Length - 1;
                    }
                    for (var _i = 0; _i < _numeroNomes; ++_i)
                    {
                        rua += _listaEndereco[_i].Trim() + " ";
                    }
                    rua = rua.Trim().ToUpper();
                }
            }
        }

        public async Task<IEnumerable<string>> ClearAll()
        {
            List<string> _erros = new();
            string _retorno;


            List<LocationResponse> _locations = (List<LocationResponse>)await _iLocationRepository.GetAll();
            foreach (var _location in _locations)
            {
                if ((_retorno = await _iLocationRepository.Delete(_location.third_id)) != "OK")
                {
                    _erros.Add(_retorno);
                }
            }
            return _erros;
        }


        public async Task CompatilizaConsultorios(List<ConsultorioResponse> consultorios, List<LocationResponse> locations, List<String> erros)
        {
            long _bairroId = 0;
            long _cidadeId = 0;

            string _retorno = "";
            string _rua = "";
            string _numero = "";
            string _nomeBairro = "";
            string _nomeCidade = "";
            long[] _vetConsultorios = new long[] {18, 36, 13, 41 };
            consultorios = consultorios.Where(i => i.AtivoWeb == 1 && _vetConsultorios.Contains(i.Id)).ToList();

            // Compatibilizar
            foreach (var consultorio in consultorios.Where(i => i.AtivoWeb == 1))
            {
                _nomeBairro = "NAO INFORMADO";
                _nomeCidade = "NAO INFORMADO";

                consultorio.Telefone = String.IsNullOrEmpty(consultorio.Telefone) ? "2122638960" : consultorio.Telefone;

                if (long.TryParse(consultorio.BairroId, out _bairroId))
                {
                    var _bairro = await _iConsultorioRepository.PegaBairro(_bairroId);
                    _nomeBairro = _bairro == null ? _nomeBairro : _bairro.Nome.Trim().ToUpper();
                }

                if (long.TryParse(consultorio.CidadeId, out _cidadeId))
                {
                    var _cidade = await _iConsultorioRepository.PegaCidade(_cidadeId);
                    _nomeCidade = _cidade == null ? _nomeCidade : _cidade.Nome.Trim().ToUpper();
                }

                // existe no boa consulta, veja se altera
                var _location = locations.Find(x => consultorio.Id.ToString() == x.third_id);
                if (_location == null)
                {
                    DesmembraEndereco(consultorio.Endereco, ref _rua, ref _numero);

                    // inclui
                    NewLocationRequest _newLocation = new()
                    {
                        id = consultorio.Id.ToString(),
                        name = consultorio.NomeFantasia.ToUpper(),

                        address = _rua,
                        number = _numero,
                        complement = consultorio.Complemento.ToUpper().Trim(),
                        neighborhood = _nomeBairro,
                        city = _nomeCidade,
                        state = consultorio.UF,
                        zip_code = consultorio.Cep.Trim(),
                        phone = consultorio.Telefone.Trim()
                    };
                    if ((_retorno = await _iLocationRepository.Create(_newLocation)) != "OK")
                    {
                        _retorno = $"[ConsultorioId={consultorio.Id}] => " + _retorno;
                        erros.Add(_retorno);
                    }
                }
                else
                {
                    DesmembraEndereco(consultorio.Endereco, ref _rua, ref _numero);

                    // altera
                    var atualiza = false;

                    if (_location.name != consultorio.NomeFantasia.ToUpper())
                        atualiza = true;

                    if (_location.address != _rua)
                        atualiza = true;

                    if (_location.number != _numero)
                        atualiza = true;

                    if (_location.complement != consultorio.Complemento.ToUpper().Trim())
                        atualiza = true;

                    if (_location.district != _nomeBairro)
                        atualiza = true;

                    if (_location.city != _nomeCidade)
                        atualiza = true;

                    if (_location.state != consultorio.UF)
                        atualiza = true;

                    if (_location.zip_code != consultorio.Cep)
                        atualiza = true;

                    if (_location.phone_region + _location.phone != consultorio.Telefone)
                        atualiza = true;

                    if (atualiza)
                    {
                        UpdateLocationRequest _updateLocation = new()
                        {
                            name = consultorio.NomeFantasia.ToUpper(),

                            address = _rua.ToUpper().Trim(),
                            number = _numero.Trim(),
                            complement = consultorio.Complemento.ToUpper().Trim(),
                            neighborhood = _nomeBairro,
                            city = _nomeCidade,
                            state = consultorio.UF,
                            zip_code = consultorio.Cep.Trim(),
                            phone = consultorio.Telefone.Trim()
                        };
                        if ((_retorno = await _iLocationRepository.Update(_location.id, _updateLocation)) != "OK")
                        {
                            _retorno = $"[ConsultorioId={consultorio.NomeFantasia}] => " + _retorno;
                            erros.Add(_retorno);
                        }
                    }
                }
            }
            foreach (var location in locations.Where(i => consultorios.Exists(c=>c.Id.ToString() == i.third_id) == false))
            {
                if ((_retorno = await _iLocationRepository.Delete(location.third_id)) != "OK")
                {
                    _retorno = $"[LocationId={location.name} deve ser excluida] => " + _retorno;
                    erros.Add(_retorno);
                }
            }
        }
    }
}
