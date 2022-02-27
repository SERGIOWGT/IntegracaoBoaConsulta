using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface IEspecialidadeAgendaService
    {
        Task<IEnumerable<EspecialidadeAgendaResponse>> GetAll();
    }
}
