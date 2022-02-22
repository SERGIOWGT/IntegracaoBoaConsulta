using System;
using System.Collections.Generic;

namespace IntegracaoBC.Domain.Mappings
{
    public class HealthCarrier
    {
        public string id { get; set; }
    }

    public record NewAgendaRequest
    {
        public string id { get; set; }
        public string doctor_id { get; set; }
        public string location_id { get; set; }
        public string private_appointment_price { get; set; }
        public string first_appointment_free { get; set; }
        public Boolean active { get; set; }
        public List<HealthCarrier> health_carriers { get; set; }
    }
}
