using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class AgendaRepository : BaseRepository, IAgendaRepository
    {
        public AgendaRepository(IConfiguration iConfiguration, ILoginBoaConsultaRepository iLoginBoaConsultaRepository) : base(iConfiguration, iLoginBoaConsultaRepository) { }

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
    }
}
