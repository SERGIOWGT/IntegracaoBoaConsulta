using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Providers.Interfaces;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class AppointmentRepository : BaseBoaConsultaRepository, IAppointmentRepository
    {
        public AppointmentRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<Tuple<string, string>> Confirm(string id)
        {
            var _retorno = "OK";
            string jsonParam = "{\"note\":\"" + id + "\"}";

            var _url = $"appointments/{id}/confirm";
            var _retornoApi = await iProviderBoaConsulta.PostAsync(jsonParam, _url);
            if (_retornoApi.Sucesso == false)
                _retorno = _retornoApi.Resultado;

            return new Tuple<string, string> (_retorno, "");
        }
    }
}
