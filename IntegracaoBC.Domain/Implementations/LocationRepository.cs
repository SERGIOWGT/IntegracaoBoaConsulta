using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IntegracaoBC.Domain.Implementations;
using Newtonsoft.Json;
using IntegracaoBC.Providers.Interfaces;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class LocationRepository : BaseBoaConsultaRepository, ILocationRepository
    {
        public LocationRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<IEnumerable<LocationResponse>> GetAll()
        {
            var _resp = await iProviderBoaConsulta.GetAsync("locations");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<BoaConsultaResponse<LocationResponse>>(_resp.Resultado);
            return (IEnumerable<LocationResponse>)_retorno.objects;
        }
        
        public async Task<string> Create(NewLocationRequest novo)
        {
            var _retorno = "OK";
            var _jsonParam = JsonConvert.SerializeObject(novo);
            var _resp = await iProviderBoaConsulta.PostAsync(_jsonParam, "locations");

            if (_resp.Sucesso == false)
                _retorno = _resp.Resultado;

            return _retorno;
        }


        public async Task<string> Update(string id, UpdateLocationRequest update)
        {
            var _retorno = "OK";

            var _jsonParam = JsonConvert.SerializeObject(update);
            var _resp = await iProviderBoaConsulta.PostAsync(_jsonParam, $"locations/{id}");
            if (_resp.Sucesso == false)
                _retorno = _resp.Resultado;

            return _retorno;
        }


        public async Task<string> Delete(string id)
        {
            var _retorno = "OK";
            var _resp = await iProviderBoaConsulta.DeleteAsync($"locations/{id}");
            if (_resp.Sucesso == false)
                _retorno = _resp.Resultado;

            return _retorno;
        }
    }
}
