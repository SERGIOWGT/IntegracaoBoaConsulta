

namespace IntegracaoBC.Domain.Mappings
{
    public record GroupSpecialtyResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int order { get; set; }
        public bool rqe_required { get; set; }
    }
}
