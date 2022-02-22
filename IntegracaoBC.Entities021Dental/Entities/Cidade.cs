using IntegracaoBC.Entities021Dental.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracaoBC.Entities021Dental.Entities
{
    public class Cidade
    {
        [Key]
        public string CidadeId { get; set; }

        [Required, MaxLength((int)TamanhoPadraoCamposEnum.NomePadrao)]
        public string Nome { get; set; }

        [Required, MaxLength((int)TamanhoPadraoCamposEnum.UF)]
        public string UF { get; set; }
    }
}
