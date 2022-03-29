using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class AgendaRepository : BaseBoaConsultaRepository, IAgendaRepository
    {

        public AgendaRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<string> Create(NewAgendaRequest novo)
        {
            var _retorno = "OK";
            var _jsonParam = JsonConvert.SerializeObject(novo);
            var _resp = await iProviderBoaConsulta.PostAsync(_jsonParam, "agendas");

            if (_resp.Sucesso == false)
                _retorno = _resp.Resultado;

            return _retorno;
        }

        public async Task<AgendaResponse> Existe(string id)
        {
            var _resp = await iProviderBoaConsulta.GetAsync($"agendas/{id}");
            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<AgendaResponse>(_resp.Resultado);
            return _retorno;
        }
    }
}