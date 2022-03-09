
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            urlPadrao = this.iConfiguration["ConfigApiDental021:UrlBase"];
            token = this.iConfiguration["ConfigApiDental021:Token"];
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
                    var _msgErro = $"Erro na chamada a API({url}).  ";

                    if (_resultContent != null)
                    {
                        if (_resultContent.Contains("erros"))
                        {
                            var _erros = JsonConvert.DeserializeObject<ErrosResponse>(_resultContent);
                            _msgErro += $"Mensagem={_erros.Erros[0].Mensagem}]";
                            throw new Exception(_msgErro);
                        }
                    }
                    _msgErro += $"[HttpStatus={_response.StatusCode}] Mensagem={_response.ReasonPhrase}]";
                    throw new Exception(_msgErro);
                }
                
                _retorno = _resultContent;

            }
            catch //(WebException ex)
            {
                //_retorno = $"[Error=D021] [Status={ex.Status}] [Message={ex.Message}";
                //throw new Exception(_retorno);
                throw;
            }
            return _retorno;
        }
    }
}
