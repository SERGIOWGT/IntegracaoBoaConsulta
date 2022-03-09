using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Provider.Agenda021
{
    public interface IProviderAgenda021
    {
        Task<string> GetAsync(string url);
    }
}
