using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class BaseRepository
    {
        protected readonly IConfiguration _iConfiguration;
        protected readonly ILoginBoaConsultaRepository _iLoginBoaConsultaRepository;

        protected readonly string urlPadrao = "";

        protected string tokenAcesso = "";

        public BaseRepository(IConfiguration iConfiguration, ILoginBoaConsultaRepository iLoginBoaConsultaRepository)
        {
            _iConfiguration = iConfiguration;
            _iLoginBoaConsultaRepository = iLoginBoaConsultaRepository;

            urlPadrao = _iConfiguration["ConfigApiBoaConsulta:UrlBase"];
        }

        protected async Task<string> PostNew(string token, string url, string jsonParams, string mensagemPadrao)
        {
            string _retorno = "OK";

            if (tokenAcesso == "")
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();

            try
            {
                var _httpContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var response = client.PostAsync(new Uri(url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    if (result == "")
                        _retorno = $"{mensagemPadrao} [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";
                    else
                    {
                        ErrorBoaConsulta _erro = JsonConvert.DeserializeObject<ErrorBoaConsulta>(result);
                        _retorno = _erro.error;
                    }
                }
            }
            catch (WebException ex)
            {
                _retorno = $"WebException: {mensagemPadrao} [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception: {mensagemPadrao} [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
    }
}
