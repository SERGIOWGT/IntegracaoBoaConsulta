
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.Dental021
{
    public interface IProvider021Dental
    {
        Task<string> GetAsync(string url);
    }
}
