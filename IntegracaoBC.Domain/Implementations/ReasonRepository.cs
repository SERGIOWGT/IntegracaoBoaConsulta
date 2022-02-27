using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.BoaConsulta;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class ReasonRepository : BaseBoaConsultaRepository, IReasonRepository
    {
        public ReasonRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }

        public async Task<IEnumerable<ReasonResponse>> Get(string specialtyId)
        {
            try
            {
                var _url = $"export/specialties/{specialtyId}/reasons";
                var _resp = await iProviderBoaConsulta.GetAsync2(_url);
                var _retorno = JsonConvert.DeserializeObject<BoaConsultaResponse<ReasonResponse>>(_resp);
                return (IEnumerable<ReasonResponse>)_retorno.objects;
            }
            catch
            {
                throw;
            }
        }
    }
}
