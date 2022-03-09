using System;
using System.Collections.Generic;

namespace IntegracaoBC.Domain.Mappings
{
    public record HealthCarrier
    {
        public string id { get; set; }
    }

    public record NewAgendaRequest
    {
        public string id { get; set; }
        public string doctor_id { get; set; }
        public string location_id { get; set; }
        public Double private_appointment_price { get; set; }
        public Boolean first_appointment_free { get; set; }
        public Boolean active { get; set; }
        public List<HealthCarrier> health_carriers { get; set; }
    }


    public record AgendaResponse
    {
        public string id { get; set; }
        public string third_id { get; set; }
        public Boolean active { get; set; }
    }

    public record UpdateAgendaRequest
    {
        public Double private_appointment_price { get; set; }
        public Boolean first_appointment_free { get; set; }
        public Boolean active { get; set; }
    }

    
}
