using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface IAgendaService
    {
        Task<IEnumerable<string>> Sync();
    }
}
