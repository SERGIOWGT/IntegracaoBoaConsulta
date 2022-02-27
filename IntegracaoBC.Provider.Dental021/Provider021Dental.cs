
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.Dental021
{
    public class Provider021Dental : IProvider021Dental
    {

        protected readonly IConfiguration iConfiguration;
        private string token = "";
        private readonly string urlPadrao = "";

        public Provider021Dental(IConfiguration iConfiguration)
        {
            this.iConfiguration = iConfiguration;

            urlPadrao = this.iConfiguration["ConfigApi021Dental:UrlBase"];
            token = this.iConfiguration["ConfigApi021Dental:Token"];
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
