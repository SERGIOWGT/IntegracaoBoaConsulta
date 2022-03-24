using IntegracaoBC.Domain.Mappings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IAgenda021Repository
    {
        Task<IEnumerable<DataSlotResponse>> ListaVagas(long expedienteId, long especialidadeAgendaId, long consultorioId, DateTime DataInicio, DateTime DataFim);
        Task<Tuple<string, long>> MarcaConsulta(MarcaConsultaRequest novo);
    }
}
