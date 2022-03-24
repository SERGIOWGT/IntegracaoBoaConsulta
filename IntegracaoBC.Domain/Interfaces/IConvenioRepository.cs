
using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IConvenioRepository
    {
        Task<IEnumerable<ConvenioResponse>> Lista();
        Task<ConvenioSimpleResponse> ListaPorBCId(string id);
    }
}
