using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<string>> Sync();
        Task<IEnumerable<string>> ClearAll();

    }
}
