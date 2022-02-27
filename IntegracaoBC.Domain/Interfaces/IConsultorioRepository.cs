using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IConsultorioRepository
    {
        Task<IEnumerable<ConsultorioResponse>> GetAll();
        Task<CidadeResponse> PegaCidade(long id);
        Task<BairroResponse> PegaBairro(long id);
    }
}
