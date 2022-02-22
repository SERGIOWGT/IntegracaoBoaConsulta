using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public record retornoPadrao
    {
        public long total_count;
        public long num_pages;
        public List<LocationResponse> objects;
    }

    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(IConfiguration iConfiguration, ILoginBoaConsultaRepository iLoginBoaConsultaRepository) : base(iConfiguration, iLoginBoaConsultaRepository) { }

        public async Task<IEnumerable<LocationResponse>> GetAll()
        {
            try
            {
                if (tokenAcesso == "")
                {
                    tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
                }


                using var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);
                var url = new Uri(urlPadrao + "locations");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro ao recuperar locations. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<retornoPadrao>(resultContent);

                return _retorno.objects;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<string> Create(NewLocationRequest novo)
        {
            string _retorno = "OK";

            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }


            try
            {
                var _jsonParam = JsonConvert.SerializeObject(novo);
                var _httpContent = new StringContent(_jsonParam, Encoding.UTF8, "application/json");
                var _url = urlPadrao + $"locations";
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var response = client.PostAsync(new Uri(_url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    if (result == "")
                    {
                        _retorno = $"Erro na chamada a post:locations. [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";
                    }
                    else
                    {
                        ErrorBoaConsulta _erro= JsonConvert.DeserializeObject<ErrorBoaConsulta>(result);
                        _retorno = _erro.error;
                    }
                } 
            }
            catch (WebException ex)
            {
                _retorno = $"WebException ao chamar post:location. [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar post:location. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
        public async Task<string> Update(string Id, UpdateLocationRequest update)
        {
            string _retorno = "OK";
            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }
                

            try
            {
                var _jsonParam = JsonConvert.SerializeObject(update);
                var _httpContent = new StringContent(_jsonParam, Encoding.UTF8, "application/json");

                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var _url = urlPadrao + $"locations/{Id}";
                var response = client.PutAsync(new Uri(_url), _httpContent).Result;
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (result == "")
                    {
                        _retorno = $"Erro na chamada a put:locations. [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";
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
                _retorno = $"WebException ao chamar put:location. [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar put:location. [Error=21006] [Message={e.Message}";
            }

            return _retorno;
        }
        public async Task<string> Delete(string Id)
        {
            string _retorno = "OK";
            if (tokenAcesso == "")
            {
                tokenAcesso = await _iLoginBoaConsultaRepository.Autoriza();
            }

            try
            {
                using var _client = new System.Net.Http.HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAcesso);

                var _url = urlPadrao + $"locations/{Id}";
                var response = _client.DeleteAsync(new Uri(_url)).Result;
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    if (result == "")
                    {
                        _retorno = $"Erro na chamada a delete:locations. [HttpStatus={response.StatusCode}, Mensagem={response.ReasonPhrase}]";
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
                _retorno = $"WebException ao chamar put:location. [Error=21005] [Status={ex.Status}] [Message={ex.Message}";
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar put:location. [Error=21006] [Message={e.Message}";
            }

            return _retorno;
        }
    }
}
