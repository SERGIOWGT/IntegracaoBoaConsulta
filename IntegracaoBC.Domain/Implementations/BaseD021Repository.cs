using IntegracaoBC.Provider.Dental021;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class BaseD021Repository
    {
        protected readonly IProvider021Dental iProvider;

        public BaseD021Repository(IProvider021Dental iProvider)
        {
            this.iProvider = iProvider;
        }

    }

}