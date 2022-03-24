using IntegracaoBC.Provider.Interfaces;
using IntegracaoBC.Providers.DTO;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers.Interfaces
{
    public interface IProviderBoaConsulta : IProviderGet, IProviderPutPost
    {
        Task<ProviderResponse> DeleteAsync(string url);
        Task<ProviderResponse> GetAsync2(string url);
    }
}
