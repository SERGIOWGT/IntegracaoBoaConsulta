using IntegracaoBC.Domain.Enums;
using System;

namespace IntegracaoBC.Domain.Mappings
{
    public class ExpedienteDentistaAtivosResponse
    {
        public long Id;
        public StatusEnum Status;
        public DateTime DataInclusao;
        public DateTime DataInicioAtividade { get; set; }
        public DateTime? DataFimAtividade { get; set; }
        public ExpedienteResponse Expediente { get; set; }
        public DentistaResponse Dentista { get; set; }
    }
}
