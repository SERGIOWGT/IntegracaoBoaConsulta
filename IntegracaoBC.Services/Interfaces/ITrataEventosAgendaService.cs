
using IntegracaoBC.Domain.Mappings;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface ITrataEventosAgendaService
    {
        Task<Tuple<string, long>> Executa(AppointmentEventRequest request);
        Task<Tuple<string, string>> Confirma(string id);
    }
}

