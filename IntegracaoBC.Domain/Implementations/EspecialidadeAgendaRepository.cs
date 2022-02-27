
using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Provider.Dental021;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class EspecialidadeAgendaRepository : BaseD021Repository, IEspecialidadeAgendaRepository
    {

        public EspecialidadeAgendaRepository(IProvider021Dental iProvider) : base(iProvider) { }

        public async Task<IEnumerable<EspecialidadeAgendaResponse>> GetAll()
        {
            try
            {
                var _resp = await iProvider.GetAsync("EspecialidadesAgenda/ListaComReason?usuarioId=1");
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<EspecialidadeAgendaResponse>>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}
