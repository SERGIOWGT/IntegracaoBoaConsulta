using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.DTO;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class DoctorRepository : BaseBoaConsultaRepository, IDoctorRepository
    {

        public DoctorRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<string> Create(NewDoctorRequest novo)
        {
            String _retorno = "OK";
            ProviderResponse _retornoApi;

            var _jsonParam = JsonConvert.SerializeObject(novo);
            _retornoApi = await iProviderBoaConsulta.PostAsync(_jsonParam, "doctors");
            if (_retornoApi.Sucesso == false)
                throw new Exception(_retornoApi.Resultado);
            
            return _retorno;
        }
        public async Task<DoctorResponse> Existe(long id)
        {
            var _resp = await iProviderBoaConsulta.GetAsync($"doctors/{id}");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<DoctorResponse>(_resp.Resultado);
            return _retorno;
        }

        public async Task<string> Update(long id, UpdateDoctorRequest doctor)
        {
            var _retorno = "OK";
            ProviderResponse _retornoApi;

            var _jsonParam = JsonConvert.SerializeObject(doctor);
            _retornoApi = await iProviderBoaConsulta.PostAsync(_jsonParam, $"doctors/{id}/doctor_info");

            if (_retornoApi.Sucesso == false)
                throw new Exception(_retornoApi.Resultado);

            return _retorno;
        }
    }
}