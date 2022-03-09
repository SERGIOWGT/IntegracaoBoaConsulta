using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.BoaConsulta;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class DoctorRepository : BaseBoaConsultaRepository, IDoctorRepository
    {

        public DoctorRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<string> Create(NewDoctorRequest novo)
        {
            var _retorno = "OK";
            try
            {
                var _jsonParam = JsonConvert.SerializeObject(novo);
                await iProviderBoaConsulta.PostAsync(_jsonParam, "doctors");
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar new:doctors. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
        public async Task<DoctorResponse> Existe(long id)
        {
            try
            {
                var _resp = await iProviderBoaConsulta.GetAsync($"doctors/{id}");
                if (_resp.ToUpper() == "NOT.FOUND")
                    return null;

                var _retorno = JsonConvert.DeserializeObject<DoctorResponse>(_resp);


                //return (DoctorResponse)_retorno.objects;
                return _retorno;
            }
            catch
            {
                throw;
            }

        }

        public async Task<string> Update(long id, UpdateDoctorRequest doctor)
        {
            var _retorno = "OK";
            try
            {
                var _jsonParam = JsonConvert.SerializeObject(doctor);
                await iProviderBoaConsulta.PostAsync(_jsonParam, $"doctors/{id}/doctor_info");
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar update:doctors. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }

        /*
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
        */
    }
}