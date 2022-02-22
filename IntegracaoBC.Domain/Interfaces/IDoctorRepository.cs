using IntegracaoBC.Domain.Mappings;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<string> Create(NewDoctorRequest novo);
        Task<string> Update(long id, UpdateDoctorRequest doctor);
        Task<DoctorResponse> Existe(long id);
    }
}
