using IntegracaoBC.Provider.BoaConsulta.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.BoaConsulta
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
            var _loginRequest = new LoginRequest()
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
                var _logonResponse = JsonConvert.DeserializeObject<LoginResponse>(_resp);
                _retorno = _logonResponse.access_token;

            }
            catch
            {
                throw;
            }

            return _retorno;
        }
        protected async Task<string> CommandAsync(EnumCommand command, string jsonParams, string url, Boolean comAutorizacao = true)
        {
            var _retorno = "";
            var _uri = new Uri(urlPadrao + url);

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

                var _result = await _response.Content.ReadAsStringAsync();

                if (_response.IsSuccessStatusCode == false)
                    throw new Exception($"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]");

                _retorno = _result;

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
        public async Task<string> PostAsync(string jsonParams, string url)
        {
            return await CommandAsync(EnumCommand.Post, jsonParams, url);
        }
        public async Task<string> PutAsync(string jsonParams, string url)
        {
            return await CommandAsync(EnumCommand.Put, jsonParams, url);
        }
        public async Task<string> DeleteAsync(string url)
        {
            var _retorno = "";
            var _uri = new Uri(urlPadrao + url);

            try
            {
                var _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var _response = _client.DeleteAsync(_uri).Result;
                if (_response.IsSuccessStatusCode == false)
                    throw new Exception($"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]");


                _retorno = await _response.Content.ReadAsStringAsync();

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
        public async Task<string> GetAsync(string url)
        {
            var _retorno = "";
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

                if (_response.StatusCode == HttpStatusCode.NotFound)
                    return "Not.Found";

                if (_response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]");
                }
                var _resultContent = _response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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

        public async Task<string> GetAsync2(string url)
        {
            var _uri = $"{url}?client_id={clientId}&client_secret={clientSecret}&user_id={userId}&per_page=1000";
            
            return await GetAsync(_uri);
        }
    }
}
