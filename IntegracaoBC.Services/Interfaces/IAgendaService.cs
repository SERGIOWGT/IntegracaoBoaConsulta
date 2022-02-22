using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Services.Interfaces
{
    public interface IAgendaService
    {
        Task<IEnumerable<string>> Sync();
        Task<IEnumerable<string>> ClearAll();
    }
}
