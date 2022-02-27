using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Provider.Dental021;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class ConsultorioRepository : BaseD021Repository, IConsultorioRepository
    {

        public ConsultorioRepository(IProvider021Dental iProvider) : base(iProvider) { }
        public async Task<IEnumerable<ConsultorioResponse>> GetAll()
        {

            try
            {
                var _resp = await iProvider.GetAsync("Consultorios?usuarioId=1");
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<ConsultorioResponse>>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }

        }
        public async Task<CidadeResponse> PegaCidade(long id)
        {
            try
            {
                var _resp = await iProvider.GetAsync($"Cidades/{id}/ListaBasico?usuarioId=1");
                var _retorno = JsonConvert.DeserializeObject<CidadeResponse>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }

        }

        public async Task<BairroResponse> PegaBairro(long id)
        {
            try
            {
                var _resp = await iProvider.GetAsync($"Bairros/{id}/ListaBasico?usuarioId=1");
                var _retorno = JsonConvert.DeserializeObject<BairroResponse>(_resp);

                return _retorno;
            }
            catch
            {
                throw;
            }

        }
    }
}