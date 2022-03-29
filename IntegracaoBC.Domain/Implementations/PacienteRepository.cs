using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class PacienteRepository : IPacienteRepository
    {
        protected readonly IProviderAgenda021 iProvider;

        public PacienteRepository(IProviderAgenda021 iProviderAgenda021)
        {
            iProvider = iProviderAgenda021;
        }

        public async Task<IEnumerable<PacienteResponse>> Lista(string cpf, DateTime nascimento)
        {
            var _resp = await iProvider.GetAsync($"pacientes/PorCpfDataNascimento?cpf={cpf}&nascimento={nascimento:yyyy-MM-dd}");
            if (_resp.CodigoHttp == HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false) 
                throw new Exception(_resp.Resultado);
                
            return JsonConvert.DeserializeObject<IEnumerable<PacienteResponse>>(_resp.Resultado);

        }

        public async Task<string> Altera(long id, AlteraPacienteRequest update)
        {
            var _retorno = "OK";
            var _jsonParam = JsonConvert.SerializeObject(update);
            var _resp = await iProvider.PutAsync(_jsonParam, $"pacientes/{id}");

            if (_resp.Sucesso == false)
                _retorno = _resp.Resultado;

            return _retorno;
        }

        public async Task<Tuple<string, long>> Insere(CriaPacienteRequest novo)
        {
            string _retorno;
            long _pacienteId = 0;

            PacienteResponse _paciente;
            var _jsonParam = JsonConvert.SerializeObject(novo);
            var _resp = await iProvider.PostAsync(_jsonParam, $"pacientes");
            if (_resp.Sucesso == false)
                return new Tuple<string, long> (_resp.Resultado, 0);

            _paciente = JsonConvert.DeserializeObject<PacienteResponse>(_resp.Resultado);
            _pacienteId = _paciente.Id;
            _retorno = "OK";

            return new Tuple<string, long> (_retorno, _pacienteId);
        }
    }
}
