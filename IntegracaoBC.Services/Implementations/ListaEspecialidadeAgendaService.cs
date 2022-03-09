using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace IntegracaoBC.Services.Implementations
{
    public class ListaEspecialidadeAgendaService : IListaEspecialidadeAgendaService
    {

        private readonly IEspecialidadeAgendaRepository _iEspecialidadeAgendaRepository;
        private readonly IReasonRepository _iReasonRepository;
        public ListaEspecialidadeAgendaService(IEspecialidadeAgendaRepository iEspecialidadeAgendaRepository, IReasonRepository iReasonRepository)
        {
            _iEspecialidadeAgendaRepository = iEspecialidadeAgendaRepository;
            _iReasonRepository = iReasonRepository;
        }


        public async Task<IEnumerable<EspecialidadeAgendaResponse>> Executa()
        {
            var _especialidadesAgenda = (List<EspecialidadeAgendaResponse>)await _iEspecialidadeAgendaRepository.GetAll();


            string _SpecialtyIdOld = "";
            foreach (var _especialidadeAgenda in _especialidadesAgenda.OrderBy(x => x.SpecialtyId))
            {
                List<ReasonResponse> _reasons = new();
                if (_SpecialtyIdOld != _especialidadeAgenda.SpecialtyId)
                {
                    _reasons = (List<ReasonResponse>)await _iReasonRepository.Get(_especialidadeAgenda.SpecialtyId);
                }
                var _reason = _reasons.Find(x => x.id == _especialidadeAgenda.ReasonId);
                if (_reason != null)
                {
                    _especialidadeAgenda.ReasonName = _reason.name;
                    _especialidadeAgenda.ReasonFullName = _reason.full_name;
                }
            }

            return _especialidadesAgenda;
        }
    }
}
