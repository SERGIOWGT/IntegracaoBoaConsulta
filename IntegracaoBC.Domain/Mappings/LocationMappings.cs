using System;
using System.Collections.Generic;

namespace IntegracaoBC.Domain.Mappings
{
   
    public record Errors
    {
        public string Message021 { get; set; }
        public string ErrorBoaConsulta { get; set; }
        public string ErrorHttps { get; set; }
    }

    public record NewLocationRequest
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string phone { get; set; }
        //public string cnpj { get; set; }
        //public string cnes { get; set; }
        //public string fixed_phone { get; set; }
        //public string whatsapp_phone { get; set; }
    }
    public record UpdateLocationRequest
    {
        public string name { get; set; }
        public string address { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string phone { get; set; }
        //public string cnpj { get; set; }
        //public string cnes { get; set; }
        //public string fixed_phone { get; set; }
        //public string whatsapp_phone { get; set; }
    }
    public record Coordinates
    {
        public double? lng { get; set; }
        public double? lat { get; set; }
    }

    public record LocationResponse
    {
        public string id { get; set; }
        public string third_id { get; set; }
        public string name { get; set; }
        public string cnpj { get; set; }
        public string cnes { get; set; }
        public string address { get; set; }
        public string number { get; set; }
        public string? reference { get; set; }
        public string complement { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string phone { get; set; }
        public string phone_region { get; set; }
        public Coordinates coordinates { get; set; }
        public List<string> additional_phones { get; set; }
        public string location_type { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
