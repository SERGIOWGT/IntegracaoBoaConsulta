using System.Net;

namespace IntegracaoBC.Provider.Agenda021.DTO
{
    public record Retorno
    {
        public bool Sucesso { get; set; }
        public HttpStatusCode CodigoHttp { get; set; }
        public string Resultado { get; set; }
    }
}
