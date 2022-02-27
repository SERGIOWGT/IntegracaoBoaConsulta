using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Provider.BoaConsulta;
using Newtonsoft.Json;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class LocationRepository : BaseBoaConsultaRepository, ILocationRepository
    {
        public LocationRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<IEnumerable<LocationResponse>> GetAll()
        {
            try
            {
                var _resp = await iProviderBoaConsulta.GetAsync("locations");
                var _retorno = JsonConvert.DeserializeObject<BoaConsultaResponse<LocationResponse>>(_resp);
                return (IEnumerable<LocationResponse>)_retorno.objects;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<string> Create(NewLocationRequest novo)
        {
            var _retorno = "OK";
            try
            {
                var _jsonParam = JsonConvert.SerializeObject(novo);
                await iProviderBoaConsulta.PostAsync(_jsonParam, "locations");
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar new:location. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }


        public async Task<string> Update(string id, UpdateLocationRequest update)
        {
            var _retorno = "OK";
            try
            {
                var _jsonParam = JsonConvert.SerializeObject(update);
                await iProviderBoaConsulta.PostAsync(_jsonParam, $"locations/{id}");
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar new:location. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }


        public async Task<string> Delete(string id)
        {
            var _retorno = "OK";
            try
            {
                await iProviderBoaConsulta.DeleteAsync($"locations/{id}");
            }
            catch (Exception e)
            {
                _retorno = $"Exception ao chamar delete:location. [Error=21006] [Message={e.Message}";
            }
            return _retorno;
        }
        /*

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
        */
        
    }
}
