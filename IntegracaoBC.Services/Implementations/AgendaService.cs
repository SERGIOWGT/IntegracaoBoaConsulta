/*
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
        private readonly IDoctorRepository _iDoctorRepository;
        private readonly IAgendaRepository _iAgendaRepository;
        private readonly IExpedienteDentistaRepository _iExpedienteRepository;
        private readonly IWorkScheduleRepository _iWorkScheduleRepository;
        public AgendaService(IDoctorRepository iDoctorRepository, IExpedienteDentistaRepository iExpedienteRepository, IAgendaRepository iAgendaRepository, IWorkScheduleRepository iWorkScheduleRepository)
        {
            _iExpedienteRepository = iExpedienteRepository;
            _iDoctorRepository = iDoctorRepository;
            _iAgendaRepository = iAgendaRepository;
            _iWorkScheduleRepository = iWorkScheduleRepository;
        }

        public Task<IEnumerable<string>> ClearAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> Sync()
        {

            // reasons
            //55e9bcae3242696a7d000036 == > Dentista (Clínico Geral)
            //5769504a12953948870006fc ==> Dentista (Dentística) - clareamento dental
            //57e5ec3893e2cb4834000001 ==> Dentista (Dentística) - atendimento geral (rotina)
            //5772d6e712953925600002b4 ==> Dentista (Dentística) - Restauração Dental


            List<string> _erros = new();

            List<ExpedienteDentistaAtivosResponse> _dentistasExpedientes = (List<ExpedienteDentistaAtivosResponse>)_iExpedienteRepository.ListaAtivos();
            if (_dentistasExpedientes.Count == 0)
            {
                _erros.Add("Não há expedientes cadastrados na base da 021 Dental");
                return _erros;
            }

            // 1o. syncroniza todos os doctors
            List<DentistaResponse> _dentistas = new();
            foreach (var _dentistaExpediente in _dentistasExpedientes.Where(x => x.Status == StatusEnum.Ativo && x.Dentista.Status == StatusEnum.Ativo && x.Expediente.ConsultorioId == 18 && x.Expediente.EspecialidadeAgendaId == 1))
            {

                if (_dentistas.FindIndex(x => x.Id == _dentistaExpediente.Dentista.Id) == -1)
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

                    // Inclui o work_schedule
                    var _newWorkSchedule = new NewWorkScheduleRequest()
                    {
                        id = _dentistaConsultorio.Id.ToString(),
                        start_date = _dentistaConsultorio.DataInicioAtividade.ToString("yyyy-MM-dd"),
                        end_date = _dentistaConsultorio.DataInicioAtividade.AddMonths(6).ToString("yyyy-MM-dd"),
                        start_time = _dentistaConsultorio.Expediente.HoraInicio.Substring(0, 5),
                        end_time = _dentistaConsultorio.Expediente.HoraFim.Substring(0, 5),
                        duration = 20,
                        weekdays = ToEnglish(_dentistaConsultorio.Expediente.DiaSemana)
                    };
                    var _retorno = "";
                    try
                    {
                        _retorno = await _iWorkScheduleRepository.Create(_agendaId, _newWorkSchedule);
                    }
                    catch (Exception e)
                    {
                        _retorno = e.Message;
                    }
                    if (_retorno != "OK")
                        _erros.Add(_retorno);
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
    }
}
*/