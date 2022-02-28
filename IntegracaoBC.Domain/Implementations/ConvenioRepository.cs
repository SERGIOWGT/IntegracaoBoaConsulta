using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Provider.Dental021;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class ConvenioRepository : BaseD021Repository, IConvenioRepository
    {

        public ConvenioRepository(IProvider021Dental iProvider) : base(iProvider) { }
        public async Task<IEnumerable<ConvenioResponse>> GetAll()
        {

            try
            {
                var _resp = await iProvider.GetAsync("Convenios?usuarioId=1");
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<ConvenioResponse>>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }

        }
    }
}
