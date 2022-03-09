using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Implementations
{
    public class LIstaVagasService : IListaVagasService
    {
        private readonly IAgenda021Repository _iAgenda021Repository;
        private readonly IExpedienteRepository _iExpedienteRepository;

        public LIstaVagasService(IAgenda021Repository iAgenda021Repository, IExpedienteRepository iExpedienteRepository)
        {
            _iAgenda021Repository = iAgenda021Repository;
            _iExpedienteRepository = iExpedienteRepository;
        }


        public async Task<ListaVagasResponse> Executa(string agendaId, long expedienteId, long dentistaId, DateTime dataInicio, DateTime dataFim)
        {

            // verifica se o expediente existe 
            var _expediente = await _iExpedienteRepository.Lista(expedienteId);
            if (_expediente == null)
                throw new Exception($"Expediente não encontrado. [Id={expedienteId}]");

            // não checa nada do dentista pois pode ter mudado na agenda e não ter sido atualizado no boaconsulta

            // Pega os horários
            var _aux = (List<DataSlotResponse>) await _iAgenda021Repository.ListaVagas(expedienteId, _expediente.EspecialidadeAgendaId, _expediente.ConsultorioId, dataInicio, dataFim);

            var _retorno = new ListaVagasResponse();
            _retorno.agenda_id = agendaId;
            _retorno.total_count = _aux.Count;
            _retorno.date_slots = _aux;


            return _retorno;
        }
    }
}
