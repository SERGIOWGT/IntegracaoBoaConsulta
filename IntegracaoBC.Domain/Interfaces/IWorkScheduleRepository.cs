using IntegracaoBC.Domain.Mappings;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IWorkScheduleRepository
    {
        Task<string> Create(string id, NewWorkScheduleRequest novo);
    }
}
