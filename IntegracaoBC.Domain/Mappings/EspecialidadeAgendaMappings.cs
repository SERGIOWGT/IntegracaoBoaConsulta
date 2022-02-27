
namespace IntegracaoBC.Domain.Mappings
{
    public class EspecialidadeAgendaResponse
    {

        public long Id;
        public string Nome { get; set; }
        public string SpecialtyId { get; set; }
        public string ReasonId { get; set; }
        public string ReasonName { get; set; }
        public string ReasonFullName { get; set; }

    }
}
