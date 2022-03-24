using IntegracaoBC.Providers.Interfaces;

namespace IntegracaoBC.Domain.Implementations
{
    public class BaseBoaConsultaRepository
    {
        protected readonly IProviderBoaConsulta iProviderBoaConsulta;

        public BaseBoaConsultaRepository(IProviderBoaConsulta iProviderBoaConsulta)
        {
            this.iProviderBoaConsulta = iProviderBoaConsulta;
        }
    }
}
