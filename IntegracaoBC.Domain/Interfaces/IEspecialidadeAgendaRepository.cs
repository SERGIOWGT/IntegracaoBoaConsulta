using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IEspecialidadeAgendaRepository
    {
        Task<IEnumerable<EspecialidadeAgendaResponse>> GetAll();
    }
}
