using IntegracaoBC.Providers.DTO;
using IntegracaoBC.Providers.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers.Implementations
{
    public class ProviderAgenda021 : Provider021, IProviderAgenda021
    {
        protected enum EnumCommand
        {
            Put,
            Post

        }

        private record Erro
        {
            public string chave { get; set; }
            public string mensagem { get; set;}
        }


        public ProviderAgenda021(IConfiguration iConfiguration) : base(iConfiguration, "ConfigApiAgenda021") { }
        
        public async Task<ProviderResponse> GetAsync(string url)
        {
            var _retorno = ValidaChamada(url);
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
        protected async Task<ProviderResponse> CommandAsync(EnumCommand command, string jsonParams, string url, Boolean comAutorizacao = true)
        {
            var _retorno = ValidaChamada(url);
            if (_retorno.Sucesso == false)
                return _retorno;

            _retorno.Sucesso = false;
            _retorno.CodigoHttp = HttpStatusCode.BadRequest;
            _retorno.Resultado = "";

            var _uri = new Uri(urlPadrao + url);
            try
            {
                var _httpContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
                var _client = new HttpClient();
                HttpResponseMessage _response = null;
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
                if (_response.IsSuccessStatusCode)
                {
                    if (_response.StatusCode == HttpStatusCode.NoContent)
                    {
                        _retorno.Resultado = "";
                        return _retorno;
                    }
                    _retorno.Resultado = await _response.Content.ReadAsStringAsync();
                    return _retorno;
                }

                var _result = await _response.Content.ReadAsStringAsync();
                if (_result == "")
                    _retorno.Resultado = $"Erro na chamada a API({url}) . [HttpStatus={_response.StatusCode}, Mensagem={_response.ReasonPhrase}]";
                else if (_result.Contains("chave"))
                {
                    var _erros = JsonConvert.DeserializeObject<List<Erro>>(_result);
                    if (_erros.Count > 0)
                        _retorno.Resultado = _erros[0].mensagem;
                    else
                        _retorno.Resultado = "Erro não identificado. [Error=21305] ";
                } else 
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
            return await CommandAsync(EnumCommand.Post, jsonParams, url);
        }
        public async Task<ProviderResponse> PutAsync(string jsonParams, string url)
        {
            return await CommandAsync(EnumCommand.Put, jsonParams, url);
        }
    }
}
