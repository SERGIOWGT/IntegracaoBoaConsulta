using IntegracaoBC.Domain.Mappings;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IExpedienteRepository
    {
        Task<ExpedienteResponse> Lista(long id);
    }
}
