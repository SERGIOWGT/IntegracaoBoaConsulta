using IntegracaoBC.Domain.Enums;
using System;

namespace IntegracaoBC.Domain.Mappings
{
    public record ConsultorioResponse
    {
        public long Id;
        public string Nome { get; set; }
        public string NomeFantasia { get; set; }
        public string PlanosAtendidos { get; set; }
        public string UF { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Telefone { get; set; }
        public string BairroId { get; set; }
        public string CidadeId { get; set; }
        public string Cep { get; set; }
        public int AtivoWeb { get; set; }
        public StatusEnum Status;
        public DateTime DataInclusao;

    }
}
