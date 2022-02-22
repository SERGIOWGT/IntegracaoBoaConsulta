using IntegracaoBC.Domain.Mappings;
using System.Collections.Generic;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IExpedienteDentistaRepository
    {
        IEnumerable<ExpedienteDentistaAtivosResponse> ListaAtivos();
    }
}
