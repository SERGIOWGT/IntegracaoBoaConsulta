using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface ISincronizaAgendaService
    {
        Task<IEnumerable<string>> Executa();
    }
}
