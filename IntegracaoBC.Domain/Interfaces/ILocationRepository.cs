using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<LocationResponse>> GetAll();
        Task<string> Create(NewLocationRequest novo);
        Task<string> Update(string Id, UpdateLocationRequest update);
        Task<string> Delete(string Id);
    }
}
