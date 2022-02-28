using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.BoaConsulta;
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
            try
            {
                var _jsonParam = JsonConvert.SerializeObject(novo);
                await iProviderBoaConsulta.PostAsync(_jsonParam, "agendas");
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar new:doctors. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }

        public async Task<AgendaResponse> Existe(string id)
        {
            try
            {
                var _resp = await iProviderBoaConsulta.GetAsync($"agendas/{id}");
                if (_resp == "Not.Found")
                    return null;


                var _retorno = JsonConvert.DeserializeObject<AgendaResponse>(_resp);
                return _retorno;
            }
            catch
            {
                throw;
            }

        }

        /*

        public async Task<string> Create(NewAgendaRequest novo)
        {
            string _mensagemPadrao = $"Erro post:agenda.";
            string _url = urlPadrao + $"agendas";
            string _jsonParams = JsonConvert.SerializeObject(novo);

            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }

            return await PostNew(tokenAcesso, _url, _jsonParams, _mensagemPadrao);
        }
        public async Task<AgendaResponse> Existe(string id)
        {
            string _mensagemPadrao = $"Erro get:agenda. [Id={id}]";
            try
            {
                if (tokenAcesso == "")
                {
                    tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
                }


                using var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);
                var url = new Uri(urlPadrao + $"agendas/{id}");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    if (result.ReasonPhrase.ToUpper() == "Not Found".ToUpper())
                        return null;
                    else
                        throw new Exception($"{_mensagemPadrao}. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<AgendaResponse>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
        */
    }
}