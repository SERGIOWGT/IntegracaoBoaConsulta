using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class Slot021Repository : BaseD021Repository, ISlot021Repository
    {
        public Slot021Repository(IProvider021Dental iProvider) : base(iProvider) { }

        public async Task<SlotResponse> Lista(long expedienteId, DateTime data, string horario)
        {
            var _strDataHora = data.ToString("yyyy-MM-dd " + horario);
            try
            {
                var _resp = await iProvider.GetAsync($"Slots?expedienteId={expedienteId}&dataHora={_strDataHora}&usuarioId=1");

                if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                    return null;

                if (_resp.Sucesso == false)
                    throw new System.Exception(_resp.Resultado);

                var _retorno = JsonConvert.DeserializeObject<SlotResponse>(_resp.Resultado);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}
