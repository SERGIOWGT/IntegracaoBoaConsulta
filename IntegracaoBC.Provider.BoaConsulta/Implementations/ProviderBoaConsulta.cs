using IntegracaoBC.Providers.DTO;
using IntegracaoBC.Providers.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers.Implementations
{
    public class ProviderBoaConsulta : IProviderBoaConsulta
    {
        protected readonly IConfiguration iConfiguration;
        private readonly ILoggerFactory iLoggerFactory;

        protected enum EnumCommand { Put, Post }
        private string tokenAcesso = "";
        private readonly string urlPadrao = "";
        private readonly string clientId = "";
        private readonly string clientSecret = "";
        private readonly string userId = "";

        public ProviderBoaConsulta(IConfiguration iConfiguration, ILoggerFactory iLoggerFactory)
        {
            this.iConfiguration = iConfiguration;
            this.iLoggerFactory = iLoggerFactory;

            urlPadrao = iConfiguration["ConfigApiBoaConsulta:UrlBase"];
            clientId = iConfiguration["ConfigApiBoaConsulta:ClientId"];
            clientSecret = iConfiguration["ConfigApiBoaConsulta:ClientSecret"];
            userId = iConfiguration["ConfigApiBoaConsulta:UserId"];

            tokenAcesso = "";
        }
        protected async Task<ProviderResponse> ValidaAutoriza(string url)
        {
            ProviderResponse _retorno = new()
            {
                Sucesso = false,
                CodigoHttp = HttpStatusCode.BadRequest,
                Resultado = ""
            }; 
            
            if (string.IsNullOrEmpty(urlPadrao))
            {
                _retorno.Resultado = "Url padrão não informada. [ProviderBC]";
                return _retorno;
            }
            if (string.IsNullOrEmpty(clientId))
            {
                _retorno.Resultado = "ClientId não informado.";
                return _retorno;
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                _retorno.Resultado = "ClientSecret não informado.";
                return _retorno;
            }
            if (string.IsNullOrEmpty(userId))
            {
                _retorno.Resultado = "UserId não informado.";
                return _retorno;
            }
            if (string.IsNullOrEmpty(url))
            {
                _retorno.Resultado = "Url não informada. [ProviderBC]";
                return _retorno;
            }

            var _loginRequest = new LoginBCRequest()
            {
                client_id = clientId,
                client_secret = clientSecret,
                user_id = userId
            };

            // Autoriza
            var _url = "login";
            var _jsonParam = JsonConvert.SerializeObject(_loginRequest);

            _retorno = await CommandAsync(EnumCommand.Post, _jsonParam, _url, false);
            if (_retorno.CodigoHttp == HttpStatusCode.NotFound)
            {
                _retorno.Resultado = "Rotina de autorização não encontrada. [ProviderBC]";
                return _retorno;
            }

            if (_retorno.Sucesso == false)
                return _retorno;

            try
            {
                var _logonResponse = JsonConvert.DeserializeObject<LoginBCResponse>(_retorno.Resultado);
                tokenAcesso = _logonResponse.access_token;
            } 
            catch
            {
                _retorno.Resultado = "Erro no DeserializeObject<LoginBCResponse>. [ProviderBC]";
            }            

            _retorno.Sucesso = true;
            return _retorno;
        }

        public async Task<ProviderResponse> GetAsync(string url)
        {
            var _retorno = await ValidaAutoriza(url);
            if (_retorno.Sucesso == false)
                return _retorno;

            var _uri = new Uri(urlPadrao + url);
            try
            {
                using var _http = new HttpClient();
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var _response = _http.GetAsync(_uri).GetAwaiter().GetResult();
                _retorno.Sucesso = _response.IsSuccessStatusCode;
                _retorno.CodigoHttp = _response.StatusCode;
                if (_response.StatusCode == HttpStatusCode.NotFound)
                    return _retorno;

                var _resultContent = _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (_response.StatusCode != HttpStatusCode.OK)
                {
                    var _id = Guid.NewGuid();

                    _retorno.Resultado = $"[Id={_id}] Erro na API(get:{url}). Mensagem={_response.ReasonPhrase}] ";

                    var _logger = iLoggerFactory.CreateLogger<ProviderBoaConsulta>();
                    _logger.LogInformation(_retorno.Resultado);

                    return _retorno;
                }
                if ((_resultContent.Length > 9) && (_resultContent.Substring(0,9) == "<!DOCTYPE"))
                {
                    _retorno.Sucesso = false;
                    _retorno.Resultado = _resultContent;
                    return _retorno;
                }
                _retorno.Resultado = _resultContent;
            }
            catch (WebException ex)
            {
                _retorno.Resultado = $"Exception ao chamar API({url}). [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno.Resultado = $"Exception ao chamar API({url}). [Error=21006] [Message={e.Message}";
            }

            return _retorno;
        }

        protected async Task<ProviderResponse> CommandAsync(EnumCommand command, string jsonParams, string url, Boolean comAutorizacao = true)
        {
            var _uri = new Uri(urlPadrao + url);

            ProviderResponse _retorno = new()
            {
                Sucesso = false,
                CodigoHttp = HttpStatusCode.BadRequest,
                Resultado = ""
            };

            try
            {
                var _httpContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
                var _client = new HttpClient();
                HttpResponseMessage _response = null;

                if (comAutorizacao)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);
                }
                switch (command)
                {
                    case EnumCommand.Post:
                        _response = _client.PostAsync(_uri, _httpContent).Result;
                        break;
                    case EnumCommand.Put:
                        _response = _client.PutAsync(_uri, _httpContent).Result;
                        break;
                }
                _retorno.Sucesso = _response.IsSuccessStatusCode;
                _retorno.CodigoHttp = _response.StatusCode;

                var _result = await _response.Content.ReadAsStringAsync();

                if (_response.IsSuccessStatusCode == false)
                {
                     var _id = Guid.NewGuid();
                    _retorno.Resultado = $"[Id={_id}] ==> Erro na chamada a API({command}:{url}). [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]";

                    var _logger = iLoggerFactory.CreateLogger<ProviderBoaConsulta>();
                    var _erroLog = _retorno.Resultado;
                    _erroLog += Environment.NewLine;
                    _erroLog += "==> BODY: " + Environment.NewLine;
                    _erroLog += jsonParams;
                    _logger.LogInformation(_erroLog);

                    
                    return _retorno;
                }

                _retorno.Resultado = _result;
            }
            catch (WebException ex)
            {
                _retorno.Resultado = $"Exception ao chamar API({url}). [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno.Resultado = $"Exception ao chamar API({url}). [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
        public async Task<ProviderResponse> PostAsync(string jsonParams, string url)
        {

            var _retorno = await ValidaAutoriza(url);
            if (_retorno.Sucesso == false)
                return _retorno;

            return await CommandAsync(EnumCommand.Post, jsonParams, url);
        }
        public async Task<ProviderResponse> PutAsync(string jsonParams, string url)
        {
            var _retorno = await ValidaAutoriza(url);
            if (_retorno.Sucesso == false)
                return _retorno;

            return await CommandAsync(EnumCommand.Put, jsonParams, url);
        }
        public async Task<ProviderResponse> DeleteAsync(string url)
        {

            var _retorno = await ValidaAutoriza(url);
            if (_retorno.Sucesso == false)
                return _retorno;

            var _uri = new Uri(urlPadrao + url);

            try
            {
                var _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var _response = _client.DeleteAsync(_uri).Result;
                _retorno.Sucesso = _response.IsSuccessStatusCode;
                _retorno.CodigoHttp = _response.StatusCode;

                if (_response.IsSuccessStatusCode == false)
                {
                    var _id = Guid.NewGuid();
                    _retorno.Resultado = $"[Id={_id}] ==> Erro na chamada a API(delete:{url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]";

                    var _logger = iLoggerFactory.CreateLogger<ProviderBoaConsulta>();
                    _logger.LogInformation(_retorno.Resultado);

                    return _retorno;
                }
                _retorno.Resultado = await _response.Content.ReadAsStringAsync();
            }
            catch (WebException ex)
            {
                _retorno.Resultado = $"Exception ao chamar API({url}). [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno.Resultado = $"Exception ao chamar API({url}). [Error=21006] [Message={e.Message}";
            }

            return _retorno;
        }
        public async Task<ProviderResponse> GetAsync2(string url)
        {
            var _uri = $"{url}?client_id={clientId}&client_secret={clientSecret}&user_id={userId}&per_page=1000";
            
            return await GetAsync(_uri);
        }
    }
}
