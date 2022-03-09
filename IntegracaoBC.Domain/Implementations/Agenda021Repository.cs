using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.Agenda021;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class Agenda021Repository : IAgenda021Repository
    {
        protected readonly IProviderAgenda021 iProvider;

        public Agenda021Repository(IProviderAgenda021 iProviderAgenda021)
        {
            iProvider = iProviderAgenda021;
        }

        public async Task<IEnumerable<DataSlotResponse>> ListaVagas(long expedienteId, long especialidadeAgendaId, long consultorioId, DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var _url = $"agenda/ListaHorariosLivresBoaConsulta?expedienteId={expedienteId}&especialidadeAgendaId={especialidadeAgendaId}&consultorioId={consultorioId}&dataInicio={dataInicio:yyyy-MM-dd}&dataFim={dataFim:yyyy-MM-dd}";
                var _resp = await iProvider.GetAsync(_url);

                var _retorno = JsonConvert.DeserializeObject<IEnumerable<DataSlotResponse>>(_resp);
                return _retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}
