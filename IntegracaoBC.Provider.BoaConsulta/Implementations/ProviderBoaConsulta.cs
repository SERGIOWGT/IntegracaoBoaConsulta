using IntegracaoBC.Providers.DTO;
using IntegracaoBC.Providers.Interfaces;
using Microsoft.Extensions.Configuration;
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
        protected enum EnumCommand
        {
            Put,
            Post

        }

        protected readonly IConfiguration iConfiguration;
        private string tokenAcesso = "";
        private readonly string urlPadrao = "";
        private readonly string clientId = "";
        private readonly string clientSecret = "";
        private readonly string userId = "";

        public ProviderBoaConsulta(IConfiguration iConfiguration)
        {
            this.iConfiguration = iConfiguration;
            urlPadrao = iConfiguration["ConfigApiBoaConsulta:UrlBase"];
            clientId = iConfiguration["ConfigApiBoaConsulta:ClientId"];
            clientSecret = iConfiguration["ConfigApiBoaConsulta:ClientSecret"];
            userId = iConfiguration["ConfigApiBoaConsulta:UserId"];
            tokenAcesso = "";
        }
        protected async Task<string> Autoriza()
        {
            string _retorno = "OK";
            var _loginRequest = new LoginBCRequest()
            {
                client_id = clientId,
                client_secret = clientSecret,
                user_id = userId
            };

            var _url = "login";
            var _jsonParam = JsonConvert.SerializeObject(_loginRequest);
            try
            {
                var _resp = await CommandAsync(EnumCommand.Post, _jsonParam, _url, false);
                var _logonResponse = JsonConvert.DeserializeObject<LoginBCResponse>(_resp.Resultado);
                _retorno = _logonResponse.access_token;

            }
            catch
            {
                throw;
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
                    _retorno.Resultado = $"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]";
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
            if (tokenAcesso == "")
            {
                tokenAcesso = await Autoriza();
            }
            return await CommandAsync(EnumCommand.Post, jsonParams, url);
        }
        public async Task<ProviderResponse> PutAsync(string jsonParams, string url)
        {
            if (tokenAcesso == "")
            {
                tokenAcesso = await Autoriza();
            }

            return await CommandAsync(EnumCommand.Put, jsonParams, url);
        }
        public async Task<ProviderResponse> DeleteAsync(string url)
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
                var _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var _response = _client.DeleteAsync(_uri).Result;
                _retorno.Sucesso = _response.IsSuccessStatusCode;
                _retorno.CodigoHttp = _response.StatusCode;

                if (_response.IsSuccessStatusCode == false)
                {
                    _retorno.Resultado = $"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]";
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
                if (tokenAcesso == "")
                {
                    tokenAcesso = await Autoriza();
                }

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
                    _retorno.Resultado = $"Erro na chamada a API({url}). Mensagem={_response.ReasonPhrase}]";
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

        public async Task<ProviderResponse> GetAsync2(string url)
        {
            var _uri = $"{url}?client_id={clientId}&client_secret={clientSecret}&user_id={userId}&per_page=1000";
            
            return await GetAsync(_uri);
        }
    }
}
