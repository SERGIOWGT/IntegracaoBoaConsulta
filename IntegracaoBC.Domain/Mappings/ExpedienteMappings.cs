using IntegracaoBC.Domain.Enums;
using System;

namespace IntegracaoBC.Domain.Mappings
{
    public class ExpedienteResponse
    {
        public long Id;
        public long ConsultorioId { get; set; }
        public string NomeConsultorio { get; set; }
        public long EspecialidadeAgendaId { get; set; }
        public string NomeEspecialidadeAgenda { get; set; }
        public long DentistaId { get; set; }
        public string NomeDentista { get; set; }
        public long StatusExpedienteId { get; set; }
        public string NomeStatusExpediente { get; set; }
        public int DiaSemana { get; set; }
        public string Periodicidade { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }
        public string HoraInicioAlmoco { get; set; }
        public string HoraFimAlmoco { get; set; }
        public DateTime DataInicioAtividade { get; set; }
        public DateTime? DataFimAtividade { get; set; }
        public int TempoConsulta { get; set; }
        public DateTime? DataUltimoSlot { get; set; }
        public DateTime? DataUltimoAgendamento { get; set; }
        public StatusEnum Status;
        public DateTime DataInclusao;
    }
}
