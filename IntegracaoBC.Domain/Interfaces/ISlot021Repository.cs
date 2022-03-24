using IntegracaoBC.Domain.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain.Interfaces
{
    public interface ISlot021Repository
    {
        Task<SlotResponse> Lista(long expedienteId, DateTime Data, string Horario);

    }
}
