using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IDentistaRepository
    {
        IEnumerable<DentistaResponse> ListaComAgenda();
    }
}
