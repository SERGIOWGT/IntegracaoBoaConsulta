using IntegracaoBC.Providers.DTO;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers.Interfaces
{
    public interface IProviderPutPost
    {
        Task<ProviderResponse> PostAsync(string jsonParams, string url);
        Task<ProviderResponse> PutAsync(string jsonParams, string url);
    }
}
