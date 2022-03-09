using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface ILimpaConsultoriosService
    {
        Task<IEnumerable<string>> Executa();
    }
}
