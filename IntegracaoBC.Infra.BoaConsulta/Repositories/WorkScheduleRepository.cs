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
    public class WorkScheduleRepository : BaseRepository, IWorkScheduleRepository
    {
        public WorkScheduleRepository(IConfiguration iConfiguration, ILoginBoaConsultaRepository iLoginBoaConsultaRepository) : base(iConfiguration, iLoginBoaConsultaRepository) { }

        public async Task<string> Create(string id, NewWorkScheduleRequest novo)
        {
            string _retorno = "OK";
            string _mensagemPadrao = $"Erro post:workschedule.";

            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }
            try
            {
                var _jsonParam = JsonConvert.SerializeObject(novo);
                var _httpContent = new StringContent(_jsonParam, Encoding.UTF8, "application/json");
                var _url = urlPadrao + $"agendas/{id}/work_schedules";
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var response = client.PostAsync(new Uri(_url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    if (result != "")
                    {
                        ErrorBoaConsulta _erro = JsonConvert.DeserializeObject<ErrorBoaConsulta>(result);
                        _retorno = _erro.error;
                    } else
                        _retorno = $"{_mensagemPadrao} [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";

                }
            }
            catch (WebException ex)
            {
                _retorno = $"WebException: {_mensagemPadrao} [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception: {_mensagemPadrao} [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
    }
}
