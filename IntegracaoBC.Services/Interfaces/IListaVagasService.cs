using IntegracaoBC.Domain.Mappings;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface IListaVagasService
    {
        Task<ListaVagasResponse> Executa(string agendaId, long expedienteId, long dentistaId, DateTime dataInicio, DateTime dataFim);
    }
}
