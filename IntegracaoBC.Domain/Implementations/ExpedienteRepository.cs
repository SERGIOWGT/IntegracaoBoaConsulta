
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Providers.Interfaces;
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

                if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                    return null;

                if (_resp.Sucesso == false)
                    throw new System.Exception(_resp.Resultado);

                var _retorno = JsonConvert.DeserializeObject<ExpedienteResponse>(_resp.Resultado);

                return _retorno;
            }
            catch
            {
                throw;
            }

        }
    }
}
