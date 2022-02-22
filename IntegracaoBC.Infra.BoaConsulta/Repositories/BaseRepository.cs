using IntegracaoBC.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IntegracaoBC.Infra.BoaConsulta.Repositories
{
    public class BaseRepository
    {
        protected readonly IConfiguration _iConfiguration;
        protected readonly ILoginBoaConsultaRepository _iLoginBoaConsultaRepository;

        protected readonly string urlPadrao = "";

        protected string tokenAcesso = "";

        public BaseRepository(IConfiguration iConfiguration, ILoginBoaConsultaRepository iLoginBoaConsultaRepository)
        {
            _iConfiguration = iConfiguration;
            _iLoginBoaConsultaRepository = iLoginBoaConsultaRepository;

            urlPadrao = _iConfiguration["ConfigApiBoaConsulta:UrlBase"];
        }
    }
}
