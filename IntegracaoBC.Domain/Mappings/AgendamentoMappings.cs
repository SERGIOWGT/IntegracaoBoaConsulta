using System;

namespace IntegracaoBC.Domain.Mappings
{
    public record GroupAppointmentEventRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int order { get; set; }
    }

    public class SpecialtyAppointmentEventRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        public bool @base { get; set; }
        public GroupAppointmentEventRequest group { get; set; }
    }

    public class Person
    {
        public string id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public DateTime date_of_birth { get; set; }
        public string cpf { get; set; }
        public string? email { get; set; }
    }

    public class PatientAppointmentEventRequest : Person
    {
        public string phone { get; set; }
        public string phone_status { get; set; }
        public bool email_verified { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class DependentAppointmentEventRequest : Person
    {
        public string phone { get; set; }
    }

    public class PatientDirAppointmentEventRequest : Person
    {
        public string phone_status { get; set; }
        public bool email_verified { get; set; }
        public string cel_phone { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class HealthCarrierAppointmentEventRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string old_slug { get; set; }
        public int ans_code { get; set; }
    }

    public class DataAppointmentEventRequest
    {
        public string id { get; set; }
        public DateTime updated_at { get; set; }
        public string agenda_id { get; set; }
        public DateTime date { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int duration { get; set; }
        public SpecialtyAppointmentEventRequest specialty { get; set; }
        public string rfe { get; set; }
        public PatientAppointmentEventRequest patient { get; set; }
        public DependentAppointmentEventRequest dependent { get; set; }
        public PatientDirAppointmentEventRequest patient_dir { get; set; }
        public HealthCarrier health_carrier { get; set; }
        public string? rescheduling_infos { get; set; }
        public string card_number { get; set; }
        public bool first_time { get; set; }
        public int status { get; set; }
        public string extended_status { get; set; }
        public string origin { get; set; }
        public string actor { get; set; }
        public string tenant { get; set; }
        public object rsvp { get; set; }
        public object patient_attended { get; set; }
        public object cancelation_option { get; set; }
        public string? cancelation_note { get; set; }
        public bool historic { get; set; }
        public bool private_appointment { get; set; }
        public string cpf { get; set; }
        public string plan_name { get; set; }
        public string? patient_note { get; set; }
        public string? doctor_note { get; set; }
        public DateTime? response_time { get; set; }
        public DateTime? confirmed_at { get; set; }
        public DateTime created_at { get; set; }
    }

    public class AppointmentEventRequest
    {
        public string @event { get; set; }
        public DataAppointmentEventRequest data { get; set; }
    }

}