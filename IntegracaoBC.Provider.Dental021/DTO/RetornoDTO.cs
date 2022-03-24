using System.Net;

namespace IntegracaoBC.Provider.Dental021.DTO
{
    public record RetornoDTO
    {
        public bool Sucesso { get; set; }
        public HttpStatusCode CodigoHttp { get; set; }
        public string Resultado { get; set; }
    }
}
