using IntegracaoBC.Domain.Interfaces;
using IntegracaoBC.Domain.Mappings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace IntegracaoBC.Infra.Dental021.Repositories
{
    public class DentistaRepository : BaseRepository, IDentistaRepository
    {
        public DentistaRepository(IConfiguration iConfiguration) : base(iConfiguration) { }

        public IEnumerable<DentistaResponse> ListaComAgenda()
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("token", token);
                var url = new Uri(urlPadrao + "Dentistas/ListaComAgenda/?usuarioId=1");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro ao recuperar dentistas. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<DentistaResponse>>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            };
        }
    }
}
