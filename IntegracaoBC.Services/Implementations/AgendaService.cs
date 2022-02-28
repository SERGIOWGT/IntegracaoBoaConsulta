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

        public AgendaService(IDoctorRepository iDoctorRepository, IExpedienteDentistaRepository iExpedienteRepository, IEspecialidadeAgendaRepository iEspecialidadeAgendaRepository)
        {
            _iExpedienteRepository = iExpedienteRepository;
            _iDoctorRepository = iDoctorRepository;
            _iEspecialidadeAgendaRepository = iEspecialidadeAgendaRepository;
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


            var _dentistasAtivos = await montaListaDentistas(_dentistasExpedientes);


            // Inclui os dentitas
            foreach (var _dentista in _dentistasAtivos)
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

            //  Pegar os convenios dos consultórios


            // Incluir as agendas
            /*
            var _consultorios = _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo && x.Expediente.ConsultorioId == 18 && x.Expediente.EspecialidadeAgendaId == 1).Select(x => x.Expediente.ConsultorioId).Distinct();
            foreach (var _consultorio in _consultorios)
            {
                var _dentistasConsultorio = _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo && x.Expediente.ConsultorioId == _consultorio && x.Expediente.EspecialidadeAgendaId == 1).OrderBy(x => x.Dentista.Id);
                long _dentistaAnterior = 0;

                foreach (var _dentistaConsultorio in _dentistasConsultorio)
                {
                    Console.WriteLine($"{_dentistaConsultorio.Expediente.ConsultorioId} => {_dentistaConsultorio.Dentista.Id} {_dentistaConsultorio.Dentista.Nome} => {_dentistaConsultorio.Expediente.DiaSemana} ");

                    var _agendaId = $"{_consultorio}_{_dentistaConsultorio.Dentista.Id}";
                    if (_dentistaAnterior != _dentistaConsultorio.Dentista.Id)
                    {
                        _dentistaAnterior = _dentistaConsultorio.Dentista.Id;

                        AgendaResponse _agenda = await _iAgendaRepository.Existe(_agendaId);
                        if (_agenda == null)
                        {
                            List<HealthCarrier> _healthCarriers = new();
                            _healthCarriers.Add(new HealthCarrier { id = "4fbd0896eed3ee0001000043" });

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
            */
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


        


    }
}
