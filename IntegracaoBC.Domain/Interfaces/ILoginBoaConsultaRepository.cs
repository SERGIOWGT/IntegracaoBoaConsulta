using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface ILoginBoaConsultaRepository
    {
        Task<string> Autoriza();
    }
}
