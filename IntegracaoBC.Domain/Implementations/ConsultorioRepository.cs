using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class ConsultorioRepository : BaseD021Repository, IConsultorioRepository
    {
        public ConsultorioRepository(IProvider021Dental iProvider) : base(iProvider) { }
        public async Task<IEnumerable<ConsultorioResponse>> GetAll()
        {

            var _resp = await iProvider.GetAsync("Consultorios?usuarioId=1");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<IEnumerable<ConsultorioResponse>>(_resp.Resultado);

            return _retorno;

        }

        public async Task<CidadeResponse> PegaCidade(long id)
        {
            var _resp = await iProvider.GetAsync($"Cidades/{id}/ListaBasico?usuarioId=1");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<CidadeResponse>(_resp.Resultado);

            return _retorno;
        }

        public async Task<BairroResponse> PegaBairro(long id)
        {
            var _resp = await iProvider.GetAsync($"Bairros/{id}/ListaBasico?usuarioId=1");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);


            var _retorno = JsonConvert.DeserializeObject<BairroResponse>(_resp.Resultado);

            return _retorno;
        }
    }
}