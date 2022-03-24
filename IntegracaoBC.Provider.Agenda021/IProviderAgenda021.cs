using IntegracaoBC.Provider.Agenda021.DTO;
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.Agenda021
{
    public interface IProviderAgenda021
    {
        Task<Retorno> GetAsync(string url);
        Task<Retorno> PostAsync(string jsonParams, string url);
        Task<Retorno> PutAsync(string jsonParams, string url);
    }
}
