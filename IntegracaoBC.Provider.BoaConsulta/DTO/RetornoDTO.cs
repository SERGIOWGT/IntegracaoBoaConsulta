using System.Net;

namespace IntegracaoBC.Providers.DTO
{
    public record ProviderResponse
    {
        public bool Sucesso { get; set; }
        public HttpStatusCode CodigoHttp { get; set; }
        public string Resultado { get; set; }
    }
}
