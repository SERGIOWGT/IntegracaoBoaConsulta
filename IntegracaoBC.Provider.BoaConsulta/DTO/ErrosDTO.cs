using System.Collections.Generic;

namespace IntegracaoBC.Providers.DTO
{
    public class Erro
    {
        public string Chave { get; set; }
        public string Mensagem { get; set; }
    }

    public class ErrosResponse
    {
        public List<Erro> Erros { get; set; }
    }
}
