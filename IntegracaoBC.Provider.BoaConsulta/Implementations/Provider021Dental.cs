using IntegracaoBC.Providers.DTO;
using IntegracaoBC.Providers.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers.Implementations
{
    public class Provider021Dental : IProvider021Dental
    {

        protected readonly IConfiguration iConfiguration;
        private string token = "";
        private readonly string urlPadrao = "";

        public Provider021Dental(IConfiguration iConfiguration)
        {
            this.iConfiguration = iConfiguration;

            urlPadrao = this.iConfiguration["ConfigApiDental021:UrlBase"];
            token = this.iConfiguration["ConfigApiDental021:Token"];
        }


        public async Task<ProviderResponse> GetAsync(string url)
        {
            ProviderResponse _retorno = new()
            {
                Sucesso = false,
                CodigoHttp = HttpStatusCode.BadRequest,
                Resultado = ""
            };

            var _uri = new Uri(urlPadrao + url);
            try
            {

                using var _http = new HttpClient();
                _http.DefaultRequestHeaders.Add("token", token);
                var _response = _http.GetAsync(_uri).GetAwaiter().GetResult();

                _retorno.Sucesso = _response.IsSuccessStatusCode;
                _retorno.CodigoHttp = _response.StatusCode;
                if (_response.StatusCode == HttpStatusCode.NotFound)
                    return _retorno;

                var _resultContent = _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (_response.StatusCode != HttpStatusCode.OK)
                {
                    var _msgErro = $"Erro na chamada a API({url}).  ";

                    if (_resultContent != null)
                    {
                        if (_resultContent.Contains("erros"))
                        {
                            var _erros = JsonConvert.DeserializeObject<ErrosResponse>(_resultContent);
                            _msgErro += $"Mensagem={_erros.Erros[0].Mensagem}]";
                            _retorno.Resultado = _msgErro;
                        }
                    }
                    _msgErro += $"[HttpStatus={_response.StatusCode}] Mensagem={_response.ReasonPhrase}]";
                    _retorno.Resultado = _msgErro;
                }
                _retorno.Resultado = _resultContent;
            }
            catch (Exception e)
            {
                _retorno.Resultado = e.Message;
            }
            return _retorno;
        }
    }
}
