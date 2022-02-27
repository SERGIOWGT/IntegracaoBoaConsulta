using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.BoaConsulta;
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
                var _retorno = JsonConvert.DeserializeObject<BoaConsultaResponse<SpecialtyResponse>>(_resp);
                return (IEnumerable<SpecialtyResponse>)_retorno.objects;
            }
            catch
            {
                throw;
            }
        }
    }
}
