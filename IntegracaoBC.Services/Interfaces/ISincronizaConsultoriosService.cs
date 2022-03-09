using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface ISincronizaConsultoriosService
    {
        Task<IEnumerable<string>> Executa();
    }
}
