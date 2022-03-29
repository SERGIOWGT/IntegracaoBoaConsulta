using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class ConvenioRepository : BaseD021Repository, IConvenioRepository
    {
        public ConvenioRepository(IProvider021Dental iProvider) : base(iProvider) { }
        public async Task<IEnumerable<ConvenioResponse>> Lista()
        {

            var _resp = await iProvider.GetAsync("Convenios?usuarioId=1");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<IEnumerable<ConvenioResponse>>(_resp.Resultado);

            return _retorno;

        }
        public async Task<ConvenioSimpleResponse> ListaPorBCId(string id)
        {
            var _resp = await iProvider.GetAsync($"Convenios/ListaPorIdBoaConsulta?boaConsultaId={id}&usuarioId=1");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);


            var _retorno = JsonConvert.DeserializeObject<ConvenioSimpleResponse>(_resp.Resultado);

            return _retorno;
        }
    }
}
