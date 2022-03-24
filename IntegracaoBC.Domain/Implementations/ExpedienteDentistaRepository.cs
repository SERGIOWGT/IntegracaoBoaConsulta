using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class ExpedienteDentistaRepository : BaseD021Repository, IExpedienteDentistaRepository
    {
        public ExpedienteDentistaRepository(IProvider021Dental iProvider) : base(iProvider) { }

        public async Task<IEnumerable<ExpedienteDentistaAtivosResponse>> ListaAtivos()
        {
            try
            {
                var _resp = await iProvider.GetAsync("ExpedientesDentistas/ListaCompletaAtivos/?usuarioId=1");

                if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                    return null;

                if (_resp.Sucesso == false)
                    throw new System.Exception(_resp.Resultado);

                var _retorno = JsonConvert.DeserializeObject<IEnumerable<ExpedienteDentistaAtivosResponse>>(_resp.Resultado);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}