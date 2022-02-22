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
    public class DoctorRepository : BaseRepository, IDoctorRepository
    {

        public DoctorRepository(IConfiguration iConfiguration, ILoginBoaConsultaRepository iLoginBoaConsultaRepository) : base(iConfiguration, iLoginBoaConsultaRepository) { }

        public async Task<string> Create(NewDoctorRequest novo)
        {
            string _retorno = "OK";
            string _mensagemPadrao = $"Erro post:doctor.";

            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }


            try
            {
                var _jsonParam = JsonConvert.SerializeObject(novo);
                var _httpContent = new StringContent(_jsonParam, Encoding.UTF8, "application/json");
                var _url = urlPadrao + $"doctors";
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var response = client.PostAsync(new Uri(_url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    if (result == "")
                    {
                        _retorno = $"{_mensagemPadrao} [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";
                    }
                    else
                    {
                        ErrorBoaConsulta _erro = JsonConvert.DeserializeObject<ErrorBoaConsulta>(result);
                        _retorno = _erro.error;
                    }
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
        public async Task<DoctorResponse> Existe(long id)
        {
            string _mensagemPadrao = $"Erro get:doctor. [Id={id}]";
            try
            {
                if (tokenAcesso == "")
                {
                    tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
                }


                using var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);
                var url = new Uri(urlPadrao + $"doctors/{id}");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    if (result.ReasonPhrase.ToUpper() == "Not Found".ToUpper())
                        return null;
                    else
                        throw new Exception($"{_mensagemPadrao}. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<DoctorResponse>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> Update(long id, UpdateDoctorRequest doctor)
        {
            string _retorno = "OK";
            string _mensagemPadrao = $"Erro post:doctor_info. [Id={id}]";

            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }

            try
            {
                var _jsonParam = JsonConvert.SerializeObject(doctor);
                var _httpContent = new StringContent(_jsonParam, Encoding.UTF8, "application/json");
                var _url = urlPadrao + $"doctors/{id}/doctor_info";
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var response = client.PostAsync(new Uri(_url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    if (result == "")
                    {
                        _retorno = $"{_mensagemPadrao} [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";
                    }
                    else
                    {
                        ErrorBoaConsulta _erro = JsonConvert.DeserializeObject<ErrorBoaConsulta>(result);
                        _retorno = $"{_mensagemPadrao}" + _erro.error;
                    }
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
