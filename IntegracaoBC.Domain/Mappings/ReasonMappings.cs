namespace IntegracaoBC.Domain.Mappings
{
    public record ReasonResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }

        public GroupSpecialtyResponse group { get; set; }
    }
}