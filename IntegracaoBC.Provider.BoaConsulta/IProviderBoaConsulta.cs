

using System.Threading.Tasks;

namespace IntegracaoBC.Provider.BoaConsulta
{
    public interface IProviderBoaConsulta
    {
        Task<string> PostAsync(string jsonParams, string url);
        Task<string> PutAsync(string jsonParams, string url);
        Task<string> DeleteAsync(string url);
        Task<string> GetAsync(string url);
        Task<string> GetAsync2(string url);

    }
}
