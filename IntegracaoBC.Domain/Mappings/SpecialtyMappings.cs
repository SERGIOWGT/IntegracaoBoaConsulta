namespace IntegracaoBC.Domain.Mappings
{
    public record SpecialtyResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public GroupSpecialtyResponse group { get; set; }
    }



    public record SpecialtyDeparaResponse
    {
        public string id { get; set; }
    }

    public record SpecialtyCompleteResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string reason_id { get; set; }
        public string reason_name { get; set; }
        public string reason_full_name { get; set; }
    }
}
