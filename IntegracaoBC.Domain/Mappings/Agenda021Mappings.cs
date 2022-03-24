namespace IntegracaoBC.Domain.Mappings
{

    public class MarcaConsultaRequest
    {
        public long PacienteId { get; set; }
        public long ConvenioId { get; set; }
        public long EspecialidadeAgendaId { get; set; }
        public long SlotId { get; set; }
        public string AgendaTerceirosId { get; set; }
    }
    public class MarcaConsultaResponse
    {
        public string Mensagem { get; set; }
        public long AgendamentoId { get; set; }
    }
}
