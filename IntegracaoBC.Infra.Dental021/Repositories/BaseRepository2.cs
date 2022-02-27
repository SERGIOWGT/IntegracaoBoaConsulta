using Microsoft.Extensions.Configuration;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class BaseRepository2
    {
        protected readonly IConfiguration _iConfiguration;

        protected readonly string urlPadrao;
        protected readonly string token;


        public BaseRepository2(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;

            urlPadrao = _iConfiguration["ConfigApi021Dental:UrlBase"];
            token = _iConfiguration["ConfigApi021Dental:Token"];
        }
    }
}
