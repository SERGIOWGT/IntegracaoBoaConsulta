using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class ConsultorioRepository : BaseRepository, IConsultorioRepository
    {

        public ConsultorioRepository(IConfiguration iConfiguration) : base (iConfiguration) {}
        public IEnumerable<ConsultorioResponse> GetAll()
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("token", token);
                var url = new Uri(urlPadrao + "Consultorios?usuarioId=1");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro ao recuperar locations. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<ConsultorioResponse>>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
        public CidadeResponse PegaCidade(long id)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("token", token);
                var url = new Uri(urlPadrao + $"Cidades/{id}/ListaBasico?usuarioId=1");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro ao recuperar locations. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<CidadeResponse>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }

        public BairroResponse PegaBairro(long id)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("token", token);
                var url = new Uri(urlPadrao + $"Bairros/{id}/ListaBasico?usuarioId=1");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro ao recuperar locations. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<BairroResponse>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            }
        }
    }
}
