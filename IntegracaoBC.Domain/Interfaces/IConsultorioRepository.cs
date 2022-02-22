using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IConsultorioRepository
    {
        IEnumerable<ConsultorioResponse> GetAll();
        CidadeResponse PegaCidade(long id);
        BairroResponse PegaBairro(long id);

    }
}
