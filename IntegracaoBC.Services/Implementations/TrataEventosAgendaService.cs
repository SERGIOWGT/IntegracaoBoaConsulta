
using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Implementations
{
    public class TrataEventosAgendaService : ITrataEventosAgendaService
    {
        private readonly IAgenda021Repository _iAgenda021Repository;
        private readonly IPacienteRepository _iPacienteRepository;
        private readonly IConvenioRepository _iConvenioRepository;
        private readonly IExpedienteRepository _iExpedienteRepository;
        private readonly ISlot021Repository _iSlotRepository;
        private readonly IAppointmentRepository _iAppointmentRepository;

        public TrataEventosAgendaService(IAgenda021Repository iAgenda021Repository, IPacienteRepository iPacienteRepository, IConvenioRepository iConvenioRepository, IExpedienteRepository iExpedienteRepository, ISlot021Repository iSlotRepository, IAppointmentRepository iAppointmentRepository)
        {
            _iAgenda021Repository = iAgenda021Repository;
            _iPacienteRepository = iPacienteRepository;
            _iConvenioRepository = iConvenioRepository;
            _iExpedienteRepository = iExpedienteRepository;
            _iSlotRepository = iSlotRepository;
            _iAppointmentRepository = iAppointmentRepository;
        }

        public async Task<Tuple<string, string>> Confirma(string id)
        {
            var (_retorno, _retorno2) = await _iAppointmentRepository.Confirm(id);

            return new Tuple<string, string> (_retorno, _retorno2);

        }

        public async Task<Tuple<string, long>> Executa(AppointmentEventRequest request)
        {
            var _retorno = "OK";
            var _pacienteRequest = request.data.patient == null ? null : request.data.patient;
            long _paciente021Id = 0;
            long _agendamentoId = 0;

            if (request.data.patient != null)
            {
                var _paciente = request.data.patient;
                if (string.IsNullOrEmpty(_paciente.cpf)) 
                    return new Tuple<string, long>("CPF não informado! [patient.cpf]", _agendamentoId);

                var _cpf = _paciente.cpf.Replace(".", "").Replace("-", "");
                if (_cpf.Length != 11)
                    return new Tuple<string, long>($"CPF inválido! [CPF={_paciente.cpf}]", _agendamentoId);

                var _sexo = _paciente.gender.ToUpper() == "FEMALE" ? "F" : _paciente.gender.ToUpper() == "MALE" ? "M" : "";
                (_retorno, _paciente021Id) = await CriaAtualizaPaciente(_cpf, _paciente.date_of_birth, _paciente.name, _sexo, _paciente.email, _paciente.phone, false);

            }
            if (_retorno != "OK")
                return new Tuple<string, long> (_retorno + $"[Cpf={request.data.patient.cpf}]", _agendamentoId);
        
            if (request.data.dependent != null)
            {
                var _paciente = request.data.dependent;

                if (string.IsNullOrEmpty(_paciente.cpf))
                    return new Tuple<string, long>("CPF não informado! [dependent.cpf]", _agendamentoId);

                var _cpf = _paciente.cpf.Replace(".", "").Replace("-", "");
                if (_cpf.Length != 11)
                    return new Tuple<string, long>($"CPF inválido! [CPF={_paciente.cpf}]", _agendamentoId);

                var _sexo = _paciente.gender.ToUpper() == "FEMALE" ? "F" : _paciente.gender.ToUpper() == "MALE" ? "M" : "";
                (_retorno, _) = await CriaAtualizaPaciente(_cpf, _paciente.date_of_birth, _paciente.name, _sexo, _paciente.email, _paciente.phone, false);
            }

            if (_retorno != "OK")
                return new Tuple<string, long> (_retorno + $"[Cpf={request.data.dependent.cpf}]", _agendamentoId);

            if (request.data.patient_dir != null)
            {
                var _paciente = request.data.patient_dir;
                if (string.IsNullOrEmpty(_paciente.cpf))
                    return new Tuple<string, long>("CPF não informado! [patient_dir.cpf]", _agendamentoId);

                var _cpf = _paciente.cpf.Replace(".", "").Replace("-", "");
                if (_cpf.Length != 11)
                    return new Tuple<string, long>($"CPF inválido! [CPF={_paciente.cpf}]", _agendamentoId);

                var _sexo = _paciente.gender.ToUpper() == "FEMALE" ? "F" : _paciente.gender.ToUpper() == "MALE" ? "M" : "";
                (_retorno, _) = await CriaAtualizaPaciente(_cpf, _paciente.date_of_birth, _paciente.name, _sexo, _paciente.email, _paciente.cel_phone, false);
            }
            if (_retorno != "OK")
                return new Tuple<string, long> (_retorno + $"[Cpf={request.data.patient_dir.cpf}]", _agendamentoId);

            long _convenioId = 1;  // Particular
            if (request.data.health_carrier != null)
            {
                // Converte o Convenio
                var _convenio = await _iConvenioRepository.ListaPorBCId(request.data.health_carrier.id);
                if (_convenio == null)
                    return new Tuple<string, long>($"Convênio não encontrado. [Id={request.data.health_carrier.id}]", _agendamentoId);

                _convenioId = _convenio.Id;
            }

            // Converte a Especialidade da Agenda
            var _expedienteDentista = request.data.agenda_id.Split("_");
            if (_expedienteDentista.Length != 3)
                return new Tuple<string, long> ($"agenda_id inválido. [Id={request.data.agenda_id}]", _agendamentoId);

            long _expedienteId = 0;
            if (long.TryParse(_expedienteDentista[0], out _expedienteId) == false)
                return new Tuple<string, long> ($"ExpedienteId inválido. [Id={_expedienteDentista[0]}]", _agendamentoId);

            var _expediente = await _iExpedienteRepository.Lista(_expedienteId);
            if (_expediente == null)
                return new Tuple<string, long> ($"Expediente não encontrado. [Id={_expedienteId}]", _agendamentoId);

            var _slot = await _iSlotRepository.Lista(_expediente.Id, request.data.date, request.data.start);
            if (_slot == null)
                return new Tuple<string, long> ($"Slot não encontrado.", _agendamentoId);

            MarcaConsultaRequest _novaConsulta = new()
            {
                SlotId = _slot.Id,
                EspecialidadeAgendaId = _expediente.EspecialidadeAgendaId,
                ConvenioId = _convenioId,
                PacienteId = _paciente021Id,
                AgendaTerceirosId = "BC_" + request.data.id
            };
            
            (_retorno, _agendamentoId) = await _iAgenda021Repository.MarcaConsulta(_novaConsulta);

            return new Tuple<string, long> (_retorno, _agendamentoId);
        }

        private async Task<Tuple<string, long>> CriaAtualizaPaciente(string cpf, DateTime dataNascimento, string nome, string sexo, string email, string celular, Boolean dependente)
        {
            string _retorno = "OK";
            string _carteirinha = "";
            long _convenioId = 0;
            long _pacienteId = 0;

            List<PacienteResponse> _pacientes = (List<PacienteResponse>)await _iPacienteRepository.Lista(cpf, dataNascimento);
            if (_pacientes == null)
            {
                CriaPacienteRequest _novo = new()
                {
                    Cpf = cpf,
                    Nome = nome,
                    DataNascimento = dataNascimento,
                    Sexo = sexo,
                    Email = email,
                    Celular = celular,
                    Carteirinha = "",
                    ConvenioId = 0,
                    Dependente = true
                };
                (_retorno, _pacienteId) = await _iPacienteRepository.Insere(_novo);


            } else {
                AlteraPacienteRequest _update = new()
                {
                    Cpf = cpf,
                    Nome = nome,
                    DataNascimento = dataNascimento,
                    Sexo = sexo,
                    Email = string.IsNullOrEmpty(email) ? _pacientes[0].Email : email,
                    Celular = string.IsNullOrEmpty(celular) ? _pacientes[0].Celular : celular,
                    Carteirinha = "",
                    ConvenioId = 0,
                    Dependente = dependente
                };
                _retorno = await _iPacienteRepository.Altera(_pacientes[0].Id, _update);

                _pacienteId = _pacientes[0].Id;
            }

            return new Tuple<string, long> (_retorno, _pacienteId);
        }


    }
}
