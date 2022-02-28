using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.Dental021;
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
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<ExpedienteDentistaAtivosResponse>>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}