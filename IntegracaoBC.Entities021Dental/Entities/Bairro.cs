using IntegracaoBC.Entities021Dental.Enums;
using System.ComponentModel.DataAnnotations;

namespace IntegracaoBC.Entities021Dental.Entities
{
    public class Bairro
    {
        [Key]
        public string BairroId { get; set; }

        [Required, MaxLength((int)TamanhoPadraoCamposEnum.NomePadrao)]
        public string Nome { get; set; }
    }
}
