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
    public class Provider021Dental : Provider021, IProvider021Dental
    {

        public Provider021Dental(IConfiguration iConfiguration) : base(iConfiguration, "ConfigApiDental021") { }

        public async Task<ProviderResponse> GetAsync(string url)
        {
            ProviderResponse _retorno = new();

            _retorno = ValidaChamada(url);
            if (_retorno.Sucesso == false)
                return _retorno;


            _retorno.Sucesso = false;
            _retorno.CodigoHttp = HttpStatusCode.BadRequest;
            _retorno.Resultado = "";


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
                            return _retorno;
                        }
                    }
                    _msgErro += $"[HttpStatus={_response.StatusCode}] Mensagem={_response.ReasonPhrase}]";
                    _retorno.Resultado = _msgErro;
                    return _retorno;
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
