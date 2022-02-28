// https://admin-integracao.boaconsulta.dev/api/v3/i/doc#!/settings/postSettingsWebhooks
// thirdsystem  bcteste


using IntegracaoBC.Domain.Enums;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace IntegracaoBC.Services.Implementations
{
    public class AgendaService : IAgendaService
    {

        private record ConsultorioConvenio
        {
            public long Id;
            public string Nome;
            public List<string> HealthCarrier;
        }


        private record EspecialidadeReason
        {
            public long EspecialidadeId;
            public List<string> Reasons;
        }


        private record DentistaReason : DentistaResponse
        {
            public List<string> Reasons;
        }


        private readonly IEspecialidadeAgendaRepository _iEspecialidadeAgendaRepository;
        private readonly IExpedienteDentistaRepository _iExpedienteRepository;
        private readonly IDoctorRepository _iDoctorRepository;
        private readonly IConsultorioRepository _iConsultorioRepository;
        private readonly IConvenioRepository _iConvenioRepository;
        private readonly IAgendaRepository _iAgendaRepository;

        public AgendaService(IDoctorRepository iDoctorRepository, IExpedienteDentistaRepository iExpedienteRepository, IEspecialidadeAgendaRepository iEspecialidadeAgendaRepository, IConsultorioRepository iConsultorioRepository, IConvenioRepository iConvenioRepository, IAgendaRepository iAgendaRepository)
        {
            _iExpedienteRepository = iExpedienteRepository;
            _iDoctorRepository = iDoctorRepository;
            _iEspecialidadeAgendaRepository = iEspecialidadeAgendaRepository;
            _iConsultorioRepository = iConsultorioRepository;
            _iConvenioRepository = iConvenioRepository;
            _iAgendaRepository = iAgendaRepository;
        }

        /*

        private readonly IAgendaRepository _iAgendaRepository;
        private readonly IExpedienteDentistaRepository _iExpedienteRepository;
        public AgendaService(IDoctorRepository iDoctorRepository, IExpedienteDentistaRepository iExpedienteRepository, IAgendaRepository iAgendaRepository)
        {
            _iExpedienteRepository = iExpedienteRepository;
            _iDoctorRepository = iDoctorRepository;
            _iAgendaRepository = iAgendaRepository;
        }
        */

        public async Task<IEnumerable<string>> Sync()
        {
            List<string> _erros = new();

            List<ExpedienteDentistaAtivosResponse> _dentistasExpedientes = (List<ExpedienteDentistaAtivosResponse>) await _iExpedienteRepository.ListaAtivos();
            if (_dentistasExpedientes.Count == 0)
            {
                _erros.Add("Não há expedientes cadastrados na base da 021 Dental");
                return _erros;
            }

            // 1o. Pegas todas as agendas
            _dentistasExpedientes = _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo && x.Dentista.Status == StatusEnum.Ativo && x.Expediente.ConsultorioId == 18 && x.Expediente.EspecialidadeAgendaId == 1).ToList();


            // Monta a lista de Dentistas
            var _dentistasAgenda = await montaListaDentistas(_dentistasExpedientes);

            // Monta a lista de Consultorios (com convenios)
            var _consultoriosAgenda = await montaListaConsultoriosAgenda(_dentistasExpedientes);


            // Inclui os dentistas
            foreach (var _dentista in _dentistasAgenda)
            {
                var _reasons = new List<Reason>();
                foreach (var _reasonId in _dentista.Reasons)
                {
                    _reasons.Add(new Reason() { id = _reasonId, rqe = null });
                }



                DoctorResponse _doctor = await _iDoctorRepository.Existe(_dentista.Id);
                if (_doctor == null)
                {
                    var _newDoctor = new NewDoctorRequest()
                    {
                        id = _dentista.Id.ToString(),
                        name = _dentista.Nome,
                        license_council = "CRO",
                        license_state = _dentista.CroUF == "" ? "RJ" : _dentista.CroUF,
                        license = _dentista.Cro,
                        reasons = _reasons
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
                    if (_retorno != "OK")
                        _erros.Add(_retorno);

                }
                else
                {
                    var _updateDoctor = new UpdateDoctorRequest()
                    {
                        name = _dentista.Nome,
                        license_council = "CRO",
                        license_state = _dentista.CroUF == "" ? "RJ" : _dentista.CroUF,
                        license = _dentista.Cro,
                        reasons = _reasons
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
                    if (_retorno != "OK")
                        _erros.Add(_retorno);
                }
            }

            // Inclui as agendas
            foreach (var _consultorio in _consultoriosAgenda)
            {
                var _dentistasConsultorio = _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo && x.Expediente.ConsultorioId == _consultorio.Id).OrderBy(x => x.Dentista.Id).ToList();
                long _dentistaAnterior = 0;

                foreach (var _dentistaConsultorio in _dentistasConsultorio)
                {
                    Console.WriteLine($"{_dentistaConsultorio.Expediente.ConsultorioId} => {_dentistaConsultorio.Dentista.Id} {_dentistaConsultorio.Dentista.Nome} => {_dentistaConsultorio.Expediente.DiaSemana} ");

                    var _agendaId = $"{_consultorio.Id}_{_dentistaConsultorio.Dentista.Id}";
                    if (_dentistaAnterior != _dentistaConsultorio.Dentista.Id)
                    {
                        _dentistaAnterior = _dentistaConsultorio.Dentista.Id;

                        AgendaResponse _agenda = await _iAgendaRepository.Existe(_agendaId);
                        if (_agenda == null)
                        {
                            List<HealthCarrier> _healthCarriers = new();
                            foreach (var _healthCarrier in _consultorio.HealthCarrier)
                            {
                                _healthCarriers.Add(new HealthCarrier { id = _healthCarrier });
                            }
                                

                            var _newAgenda = new NewAgendaRequest()
                            {
                                id = _agendaId,
                                doctor_id = _dentistaConsultorio.Dentista.Id.ToString(),
                                location_id = _dentistaConsultorio.Expediente.ConsultorioId.ToString(),
                                private_appointment_price = 150.00,
                                first_appointment_free = false,
                                active = true,
                                health_carriers = _healthCarriers
                            };
                            var _retornoAgenda = "";
                            try
                            {
                                _retornoAgenda = await _iAgendaRepository.Create(_newAgenda);
                            }
                            catch (Exception e)
                            {
                                _retornoAgenda = e.Message;
                            }
                            if (_retornoAgenda != "OK")
                                _erros.Add(_retornoAgenda);

                        } else
                        {

                        }
                    }
                }

            }
            return _erros;
        }
        private string ToEnglish(int workday)
        {
            return workday == 0 ? "sunday" :
                    workday == 1 ? "monday" :
                    workday == 2 ? "tuesday" :
                    workday == 3 ? "wednesday" :
                    workday == 4 ? "thusrday" :
                    workday == 5 ? "friday" : "saturday";
        }

        private async Task<List<EspecialidadeReason>> montaListaEspecialidadeReason(List<long> especialidadesAgenda)
        {
            var _retorno = new List<EspecialidadeReason>();
            var _especialidadesAgenda = (List<EspecialidadeAgendaResponse>)await _iEspecialidadeAgendaRepository.GetAll();

            foreach (var _especialidadeAgenda in especialidadesAgenda)
            {
                var _novo = new EspecialidadeReason();
                _novo.EspecialidadeId = _especialidadeAgenda;
                _novo.Reasons = new();

                var _reasons = _especialidadesAgenda.FindAll(x => x.Id == _especialidadeAgenda);
                foreach (var _reason in _reasons)
                {
                    _novo.Reasons.Add(_reason.ReasonId);
                }
                _retorno.Add(_novo);
            }


            return _retorno;
        }


        private async Task<List<DentistaReason>> montaListaDentistas(List<ExpedienteDentistaAtivosResponse> expedientesAtivos)
        {
            var _especialidadesReason = await montaListaEspecialidadeReason(expedientesAtivos.Select(x => x.Expediente.EspecialidadeAgendaId).Distinct().ToList());

            List<DentistaReason> _dentistas = new();


            long _dentistaOld = 0;
            foreach (var _dentistaExpediente in expedientesAtivos.OrderBy(x => x.Dentista.Id))
            {

                if (_dentistaOld != _dentistaExpediente.Dentista.Id)
                {
                    _dentistas.Add(new DentistaReason()
                    {
                        Id = _dentistaExpediente.Dentista.Id,
                        Nome = _dentistaExpediente.Dentista.Nome,
                        Cro = _dentistaExpediente.Dentista.Cro,
                        CroUF = _dentistaExpediente.Dentista.CroUF,
                        Status = _dentistaExpediente.Dentista.Status,
                        Reasons = new List<string>()
                    });
                    _dentistaOld = _dentistaExpediente.Dentista.Id;
                }
            }


            foreach (var _dentista in _dentistas)
            {
                var _especialidades = expedientesAtivos.FindAll(x => x.Dentista.Id == _dentista.Id).Select(x => x.Expediente.EspecialidadeAgendaId).Distinct().ToList();
                foreach (var _especialidade in _especialidades)
                {
                    var _aux = _especialidadesReason.Find(x => x.EspecialidadeId == _especialidade);
                    if (_aux != null)
                    {
                        foreach (var _reason in _aux.Reasons)
                        {
                            _dentista.Reasons.Add(_reason);
                        }
                    }
                }
            }

            return _dentistas;
        }

        private async Task<List<ConsultorioConvenio>> montaListaConsultoriosAgenda(List<ExpedienteDentistaAtivosResponse> expedientesAtivos)
        {

            var _retorno = new List<ConsultorioConvenio>();
            var _convenios021 = (List<ConvenioResponse>)await _iConvenioRepository.GetAll();
            var _consultorios021 = (List<ConsultorioResponse>)await _iConsultorioRepository.GetAll();
            var _consultoriosAgenda = expedientesAtivos.Select(x => x.Expediente.ConsultorioId).Distinct().ToList();


            foreach (var _consultorio in _consultoriosAgenda)
            {

                var _achouConsultorio = _consultorios021.FirstOrDefault(x => x.Id == _consultorio && x.Status == StatusEnum.Ativo);
                if (_achouConsultorio != null) 
                {
                    if (string.IsNullOrEmpty(_achouConsultorio.PlanosAtendidos) == false)
                    {
                        var _listaPlanos = _achouConsultorio.PlanosAtendidos.Split(",");
                        var _healthCarrier = new List<string>();
                        foreach (var _plano in _listaPlanos)
                        {
                            var _achouConvenio = _convenios021.FirstOrDefault(x => x.Id.ToString() == _plano && x.Status == StatusEnum.Ativo);
                            if (_achouConvenio != null)
                            {
                                if (string.IsNullOrEmpty(_achouConvenio.BoaConsultaConvenioId) == false)
                                {
                                    _healthCarrier.Add(_achouConvenio.BoaConsultaConvenioId);
                                }
                            }

                        }
                        if (_healthCarrier.Count > 0)
                        {
                            var _novo = new ConsultorioConvenio();
                            _novo.Id = _achouConsultorio.Id;
                            _novo.Nome = _achouConsultorio.Nome;
                            _novo.HealthCarrier = _healthCarrier;
                            _retorno.Add(_novo);
                        }
                    }

                }
            }
            return _retorno;
        }
    }
}
