using IntegracaoBC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IntegracaoBC.Domain.Mappings
{
    public class Reason
    {
        public string id { get; set; }
        public string? rqe { get; set; }
}

    public class Agenda
    {
        public int id { get; set; }
        public string location_id { get; set; }
        public string private_appointment_price { get; set; }
    }

    public class UpdateDoctorRequest
    {
        public string name { get; set; }
        public string license_council { get; set; }
        public string license_state { get; set; }
        public string license { get; set; }
        public List<Reason> reasons { get; set; }
        public List<Agenda> agendas { get; set; }

        //public string cpf { get; set; }
        //public string phone { get; set; }
        //public string photo { get; set; }
        //public string academic_formation { get; set; }
        //public string professional_experience { get; set; }
        //public bool active { get; set; }
        //public string gender { get; set; }
        //public bool agenda_online { get; set; }
    }
    public class NewDoctorRequest : UpdateDoctorRequest
    {
        public string id { get; set; }
        //public List<Agenda> agendas { get; set; }
    }

    public class DoctorResponse
    {
        public string id { get; set; }
        public string third_id { get; set; }
        public string name { get; set; }
        public string license_council { get; set; }
        public string license_state { get; set; }
        public string license { get; set; }
        public string? phone { get; set; }
        public string? phone_region { get; set; }
        public bool active { get; set; }
        public bool agenda_online { get; set; }
        public bool perfil_specialty { get; set; }
        public string? academic_formation { get; set; }
        public string? gender { get; set; }
        public string? professional_experience { get; set; }
        public string slug { get; set; }
        public IList<string> tenants { get; set; }
        public bool telemedicine_active { get; set; }
        public string telemedicine_price { get; set; }
        public bool telemedicine_online { get; set; }
        public DateTime approved_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        // agenda
        //public IList<Agendas> agendas { get; set; }

    }
}
