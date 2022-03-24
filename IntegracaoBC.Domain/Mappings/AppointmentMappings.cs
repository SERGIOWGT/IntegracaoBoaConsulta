using System;

namespace IntegracaoBC.Domain.Mappings
{
    public record ConfirmAppointmentResponse
    {
        public string id { get; set; }
        public string third_id { get; set; }
        public string agenda_id { get; set; }
        public DateTime date { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int duration { get; set; }
        public string card_number { get; set; }
        public bool first_time { get; set; }
        public int status { get; set; }
        public string extended_status { get; set; }
        public string origin { get; set; }
        public string actor { get; set; }
        public bool private_appointment { get; set; }
        public string cpf { get; set; }
        public string plan_name { get; set; }
        public string? patient_note { get; set; }
        public DateTime? confirmed_at { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? update_at { get; set; }
    }

}
