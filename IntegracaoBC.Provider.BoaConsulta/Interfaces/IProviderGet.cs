
using IntegracaoBC.Providers.DTO;
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.Interfaces
{
    public interface IProviderGet
    {
        Task<ProviderResponse> GetAsync(string url);
    }
}
