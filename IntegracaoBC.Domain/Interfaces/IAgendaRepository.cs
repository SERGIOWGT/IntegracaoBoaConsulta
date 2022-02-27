using IntegracaoBC.Domain.Mappings;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IAgendaRepository
    {
        Task<string> Create(NewAgendaRequest novo);
        Task<AgendaResponse> Existe(string id);
    }
}
