using System;

namespace IntegracaoBC.Domain.Mappings
{
    public class CriaPacienteRequest
    {
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Carteirinha { get; set; }
        public long ConvenioId { get; set; }
        public Boolean Dependente { get; set; }
    }

    public class AlteraPacienteRequest : CriaPacienteRequest
    {
    }

    public class PacienteResponse : CriaPacienteRequest
    {
        public long Id { get; set; }
    }

}
