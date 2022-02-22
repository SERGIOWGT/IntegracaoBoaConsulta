using IntegracaoBC.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace IntegracaoBC.Domain.Mappings
{
    public class DentistaResponse
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        [MaybeNull]
        public string Cro { get; set; }
        [MaybeNull]
        public string CroUF { get; set; }

        public StatusEnum Status;

    }
}
