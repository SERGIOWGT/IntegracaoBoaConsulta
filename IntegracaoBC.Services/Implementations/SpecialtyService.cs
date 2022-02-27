using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace IntegracaoBC.Services.Implementations
{
    public class SpecialtyService : ISpecialtyService
    {

        private readonly ISpecialtyRepository _iSpecialtyRepository;
        private readonly IReasonRepository _iReasonRepository;
        public SpecialtyService(ISpecialtyRepository iSpecialtyRepository, IReasonRepository iReasonRepository)
        {
            _iSpecialtyRepository = iSpecialtyRepository;
            _iReasonRepository = iReasonRepository;
        }


        public async Task<IEnumerable<SpecialtyCompleteResponse>> GetAll()
        {
            List<SpecialtyCompleteResponse> _retorno = new();

            var _result = (List<SpecialtyResponse>) await _iSpecialtyRepository.GetAll();
            var _especialidadesDentista = _result.Where(x => x.group.id == "55e9bc863242696a7d000004").ToList();
            _result.Clear();

            foreach (var _especialidade in _especialidadesDentista)
            {
                var _reasons = await _iReasonRepository.Get(_especialidade.id);
                foreach (var _reason in _reasons)
                {
                    var novo = new SpecialtyCompleteResponse()
                    {
                        id = _especialidade.id,
                        name = _especialidade.name,
                        slug = _especialidade.slug,
                        reason_id = _reason.id,
                        reason_name = _reason.name,
                        reason_full_name = _reason.full_name,
                    };

                    _retorno.Add(novo);
                }
            }

            return _retorno;
        }
    }
}
