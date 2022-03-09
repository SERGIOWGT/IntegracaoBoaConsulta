using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface IListaSpecialtyService
    {
        Task<IEnumerable<SpecialtyCompleteResponse>> Executa();
    }
}
