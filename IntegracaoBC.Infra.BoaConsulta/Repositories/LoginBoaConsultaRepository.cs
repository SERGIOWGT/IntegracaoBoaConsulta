using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class LoginBoaConsultaRepository : ILoginBoaConsultaRepository
    {
        private readonly IConfiguration _iConfiguration;

        private readonly string urlPadrao = "";
        private readonly string clientId = "";
        private readonly string clientSecret = "";
        private readonly string userId = "";

        public LoginBoaConsultaRepository(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;

            urlPadrao = _iConfiguration["ConfigApiBoaConsulta:UrlBase"];
            clientId = _iConfiguration["ConfigApiBoaConsulta:ClientId"];
            clientSecret = _iConfiguration["ConfigApiBoaConsulta:ClientSecret"];
            userId = _iConfiguration["ConfigApiBoaConsulta:UserId"];
        }

        public async Task<string> Autoriza()
        {
            string _retorno = "OK";
            var _loginRequest = new LoginRequest()
            {
                client_id = clientId,
                client_secret = clientSecret,
                user_id = userId
            };

            try
            {
                var _jsonParam = JsonConvert.SerializeObject(_loginRequest);
                var _httpContent = new StringContent(_jsonParam, Encoding.UTF8, "application/json");
                var _url = urlPadrao + $"login";
                var client = new System.Net.Http.HttpClient();
                var response = client.PostAsync(new Uri(_url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    LoginResponse _loginRetorno = JsonConvert.DeserializeObject<LoginResponse>(result);
                    _retorno = _loginRetorno.access_token;
                }
                else
                {
                    _retorno = result == "" ? $"Erro na chamada a API login. [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]" : result;
                }
            }
            catch (WebException ex)
            {
                _retorno = $"Exception ao chamar login. [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar login. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
    }
}
