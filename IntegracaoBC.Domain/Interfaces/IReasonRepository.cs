using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IReasonRepository
    {
        Task<IEnumerable<ReasonResponse>> Get(string specialtyId);
    }
}
