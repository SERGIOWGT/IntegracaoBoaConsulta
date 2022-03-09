
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Provider.Dental021;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class ExpedienteRepository : BaseD021Repository, IExpedienteRepository
    {

        public ExpedienteRepository(IProvider021Dental iProvider) : base(iProvider) { }

        public async Task<ExpedienteResponse> Lista(long id)
        {

            try
            {
                var _resp = await iProvider.GetAsync($"Expedientes/{id}?usuarioId=1");
                var _retorno = JsonConvert.DeserializeObject<ExpedienteResponse>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }

        }
    }
}
