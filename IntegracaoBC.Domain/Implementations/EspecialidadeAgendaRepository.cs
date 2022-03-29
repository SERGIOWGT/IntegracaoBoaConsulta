using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Providers.Interfaces;
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
            var _resp = await iProvider.GetAsync("EspecialidadesAgenda/ListaComReason?usuarioId=1");

            if (_resp.CodigoHttp == System.Net.HttpStatusCode.NotFound)
                return null;

            if (_resp.Sucesso == false)
                throw new System.Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<IEnumerable<EspecialidadeAgendaResponse>>(_resp.Resultado);

            return _retorno;
        }
    }
}
