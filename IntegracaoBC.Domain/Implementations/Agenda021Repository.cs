using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Implementations
{
    public class Agenda021Repository : IAgenda021Repository
    {
        protected readonly IProviderAgenda021 iProvider;

        public Agenda021Repository(IProviderAgenda021 iProviderAgenda021)
        {
            iProvider = iProviderAgenda021;
        }

        public async Task<Tuple<string, long>> MarcaConsulta(MarcaConsultaRequest novo)
        {
            var _retorno = "OK";
            long _agendamentoId = 0;
            var _jsonParam = JsonConvert.SerializeObject(novo);
            var _resp = await iProvider.PutAsync(_jsonParam, "agenda/Marca");
            if (_resp.Sucesso == false)
                return new Tuple<string, long> (_resp.Resultado, _agendamentoId);

            var _xxx = JsonConvert.DeserializeObject<MarcaConsultaResponse>(_resp.Resultado);
            _agendamentoId = _xxx.AgendamentoId;
            _retorno = _xxx.Mensagem;

            return new Tuple<string, long>(_retorno, _agendamentoId);
        }

        public async Task<IEnumerable<DataSlotResponse>> ListaVagas(long expedienteId, long especialidadeAgendaId, long consultorioId, DateTime dataInicio, DateTime dataFim)
        {
            var _url = $"agenda/ListaHorariosLivresBoaConsulta?expedienteId={expedienteId}&especialidadeAgendaId={especialidadeAgendaId}&consultorioId={consultorioId}&dataInicio={dataInicio:yyyy-MM-dd}&dataFim={dataFim:yyyy-MM-dd}";
            var _resp = await iProvider.GetAsync(_url);

            if (_resp.Sucesso == false)
                throw new Exception(_resp.Resultado);

            var _retorno = JsonConvert.DeserializeObject<IEnumerable<DataSlotResponse>>(_resp.Resultado);
            return _retorno;
        }
    }
}
