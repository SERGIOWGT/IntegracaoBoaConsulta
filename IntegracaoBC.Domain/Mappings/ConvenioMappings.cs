using IntegracaoBC.Domain.Enums;
namespace IntegracaoBC.Domain.Mappings
{
    public record ConvenioResponse
    {
        public long Id;
        public string Nome { get; set; }
        public string BoaConsultaConvenioId { get; set; }
        public StatusEnum Status;
    }
}
