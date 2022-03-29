
using IntegracaoBC.Providers.DTO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers.Implementations
{
    public class Provider021
    {
        protected readonly IConfiguration iConfiguration;
        protected string token = "";
        protected readonly string urlPadrao = "";

        public Provider021(IConfiguration iConfiguration, string section)
        {
            this.iConfiguration = iConfiguration;

            urlPadrao = this.iConfiguration[$"{section}:UrlBase"];
            token = this.iConfiguration[$"{section}:Token"];
        }

        public ProviderResponse ValidaChamada(string url)
        {
            ProviderResponse _retorno = new()
            {
                Sucesso = false,
                CodigoHttp = HttpStatusCode.BadRequest,
                Resultado = ""
            };

            if (string.IsNullOrEmpty(urlPadrao))
            {
                _retorno.Resultado = "Url padrão não encontrada.";
                return _retorno;
            }
            if (string.IsNullOrEmpty(token))
            {
                _retorno.Resultado = "Token de segurança não encontrado.";
                return _retorno;
            }
            if (string.IsNullOrEmpty(url))
            {
                _retorno.Resultado = "url não informada.";
                return _retorno;
            }

            _retorno.Sucesso = true;
            return _retorno;
        }
    }
}
