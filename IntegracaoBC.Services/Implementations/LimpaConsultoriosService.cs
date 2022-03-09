using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Implementations
{
    public class LimpaConsultoriosService : ILimpaConsultoriosService
    {
        private readonly ILocationRepository _iLocationRepository;
        public LimpaConsultoriosService(ILocationRepository iLocationRepository)
        {
            _iLocationRepository = iLocationRepository;
        }

        public async Task<IEnumerable<string>> Executa()
        {
            List<string> _erros = new();
            string _retorno;


            List<LocationResponse> _locations = (List<LocationResponse>)await _iLocationRepository.GetAll();
            foreach (var _location in _locations)
            {
                if ((_retorno = await _iLocationRepository.Delete(_location.third_id)) != "OK")
                {
                    _erros.Add(_retorno);
                }
            }
            return _erros;
        }
    }
}
