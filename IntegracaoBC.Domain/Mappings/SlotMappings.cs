using System;

namespace IntegracaoBC.Domain.Mappings
{

    public record SlotResponse
    {
        public long Id { get; set; }
        public long ConsultorioId { get; set; }
        public long DentistaId { get; set; }
        public long ExpedienteId { get; set; }
        public string HoraInicio { get; set; }
        public DateTime Data { get; set; }
    }
}

