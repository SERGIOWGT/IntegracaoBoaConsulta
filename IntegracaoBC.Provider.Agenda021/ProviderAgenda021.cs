using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.Agenda021
{
    public class ProviderAgenda021 : IProviderAgenda021
    {
        protected readonly IConfiguration iConfiguration;
        private string token = "";
        private readonly string urlPadrao = "";

        public ProviderAgenda021(IConfiguration iConfiguration)
        {
            this.iConfiguration = iConfiguration;

            urlPadrao = this.iConfiguration["ConfigApiAgenda021:UrlBase"];
            token = this.iConfiguration["ConfigApiAgenda021:Token"];
        }


        public async Task<string> GetAsync(string url)
        {
            var _retorno = "";
            var _uri = new Uri(urlPadrao + url);
            try
            {

                using var _http = new HttpClient();
                _http.DefaultRequestHeaders.Add("token", token);
                var _response = _http.GetAsync(_uri).GetAwaiter().GetResult();
                var _resultContent = _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (_response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]");
                }
                _retorno = _resultContent;
            }
            catch (WebException ex)
            {
                _retorno = $"Exception ao chamar API({url}). [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar API({url}). [Error=21006] [Message={e.Message}";
            }

            return _retorno;
        }
    }
}
