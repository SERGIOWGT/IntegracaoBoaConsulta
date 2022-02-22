using IntegracaoBC.Entities021Dental.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracaoBC.Entities021Dental.Entities
{
    public class ClasseBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [EnumDataType(typeof(StatusEnum), ErrorMessage = "Somente valores 1-Ativo, 0-Inativo")]
        [Display(Description = "Status do Registro")]
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;

        [Required]
        public DateTime DataInclusao { get; set; }

        public virtual string NomeClasse()
        {
            return "ClasseBase";
        }
    }
}
