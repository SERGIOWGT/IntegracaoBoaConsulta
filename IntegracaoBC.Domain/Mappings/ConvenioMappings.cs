using IntegracaoBC.Domain.Enums;
namespace IntegracaoBC.Domain.Mappings
{
    public record ConvenioSimpleResponse
    {
        public long Id;
        public string Nome { get; set; }
    }

    public record ConvenioResponse : ConvenioSimpleResponse
    {
        public string BoaConsultaConvenioId { get; set; }
        public StatusEnum Status;
    }

}
