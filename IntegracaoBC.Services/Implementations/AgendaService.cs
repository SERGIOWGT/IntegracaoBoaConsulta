using IntegracaoBC.Domain.Enums;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Implementations
{
    public class AgendaService : IAgendaService
    {
        private readonly IDoctorRepository _iDoctorRepository;
        private readonly IExpedienteDentistaRepository _iExpedienteRepository;
        public AgendaService(IDoctorRepository iDoctorRepository, IExpedienteDentistaRepository iExpedienteRepository)
        {
            _iExpedienteRepository = iExpedienteRepository;
            _iDoctorRepository = iDoctorRepository;
        }

        public Task<IEnumerable<string>> ClearAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> Sync()
        {
            List<string> _erros = new();

            List<ExpedienteDentistaAtivosResponse> _dentistasExpedientes = (List<ExpedienteDentistaAtivosResponse>)_iExpedienteRepository.ListaAtivos();
            if (_dentistasExpedientes.Count == 0)
            {
                _erros.Add("Não há expedientes cadastrados na base da 021 Dental");
                return _erros;
            }
            // 1o. syncroniza todos os doctors
            List<DentistaResponse> _dentistas = new();
            foreach (var _dentistaExpediente in _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo && x.Dentista.Status == StatusEnum.Ativo).Take(5))
            {

                if (_dentistas.FindIndex(x=>x.Id == _dentistaExpediente.Dentista.Id) == -1)
                {
                    _dentistas.Add(new DentistaResponse()
                    {
                        Id = _dentistaExpediente.Dentista.Id,
                        Nome = _dentistaExpediente.Dentista.Nome,
                        Cro = _dentistaExpediente.Dentista.Cro,
                        CroUF = _dentistaExpediente.Dentista.CroUF,
                        Status = _dentistaExpediente.Dentista.Status
                    });
                }
            }

            foreach (var _dentista in _dentistas)
            {
                DoctorResponse _doctor = await _iDoctorRepository.Existe(_dentista.Id);

                if (_doctor == null)
                {
                    // não - novo
                    var _resons = new List<Reason>();
                    _resons.Add(new Reason() { id = "55e9bcae3242696a7d000035", rqe = null });

                    var _newDoctor = new NewDoctorRequest()
                    {
                        id = _dentista.Id.ToString(),
                        name = _dentista.Nome,
                        license_council = "CRO",
                        license_state = _dentista.CroUF == "" ? "RJ" : _dentista.CroUF,
                        license = _dentista.Cro,
                        reasons = _resons
                    };
                    var _retorno = "";
                    try
                    {
                        _retorno = await _iDoctorRepository.Create(_newDoctor);
                    }
                    catch (Exception e)
                    {
                        _retorno = e.Message;
                    }
                    if (_retorno != "")
                        _erros.Add(_retorno);

                }
                else
                {
                    var _updateDoctor = new UpdateDoctorRequest()
                    {
                        name = _dentista.Nome,
                        license_council = "CRO",
                        license_state = _dentista.CroUF == "" ? "RJ" : _dentista.CroUF,
                        license = _dentista.Cro
                    };
                    // NAO ESTÁ FUNCIONANDO
                    var _retorno = "";
                    try
                    {
                        _retorno = await _iDoctorRepository.Update(_dentista.Id, _updateDoctor);
                    } 
                    catch (Exception e)
                    {
                        _retorno = e.Message;
                    }
                    if (_retorno != "")
                        _erros.Add(_retorno);
                }

            }

            foreach (var _dentistaExpediente in _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo))
            {
                // verifica se o dentista já existe
                // sim - altera

                

                // cria a agenda

                // criar o work

            }
            return _erros;
        }
    }
}
