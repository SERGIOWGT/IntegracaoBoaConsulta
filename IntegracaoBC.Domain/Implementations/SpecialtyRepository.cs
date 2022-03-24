using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class SpecialtyRepository : BaseBoaConsultaRepository, ISpecialtyRepository
    {
        public SpecialtyRepository(IProviderBoaConsulta iProviderBoaConsulta) : base(iProviderBoaConsulta) { }


        public async Task<IEnumerable<SpecialtyResponse>> GetAll()
        {
            try
            {
                var _resp = await iProviderBoaConsulta.GetAsync2("export/specialties");

                if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                    return null;

                if (_resp.Sucesso == false)
                    throw new System.Exception(_resp.Resultado);

                var _retorno = JsonConvert.DeserializeObject<BoaConsultaResponse<SpecialtyResponse>>(_resp.Resultado);
                return (IEnumerable<SpecialtyResponse>)_retorno.objects;
            }
            catch
            {
                throw;
            }
        }
    }
}
