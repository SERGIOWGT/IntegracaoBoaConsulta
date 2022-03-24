using IntegracaoBC.Domain.Mappings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<PacienteResponse>> Lista(string cpf, DateTime Nascimento);
        Task<string> Altera(long id, AlteraPacienteRequest update);
        Task<Tuple<string, long>> Insere(CriaPacienteRequest novo);
    }
}
