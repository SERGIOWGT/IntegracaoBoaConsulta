using System.Collections.Generic;

namespace IntegracaoBC.Domain.Mappings
{

    public record BoaConsultaResponse<T>
    {
        public long total_count;
        public long num_pages;
        public List<T> objects;
    }
}
