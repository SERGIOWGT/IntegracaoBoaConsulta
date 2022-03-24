using System;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Tuple<string, string>> Confirm(string id);
    }
}
