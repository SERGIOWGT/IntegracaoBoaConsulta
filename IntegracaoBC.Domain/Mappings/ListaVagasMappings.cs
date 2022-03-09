using System.Collections.Generic;

namespace IntegracaoBC.Domain.Mappings
{
    public record Slot
    {
        public string start { get; set; }
        public string end { get; set; }
        public int duration { get; set; }
    }


    public record DataSlotResponse
    {
        public string date { get; set; }
        public List<Slot> slots { get; set; }

    }

    public record ListaVagasResponse
    {
        public string agenda_id { get; set; }
        public long total_count { get; set; }
        public List<DataSlotResponse> date_slots { get; set; }
    }

    public record ListaVagasRequest
    {
        public string agenda_id { get; set; }
        public string location_id { get; set; }
        public string doctor_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}
