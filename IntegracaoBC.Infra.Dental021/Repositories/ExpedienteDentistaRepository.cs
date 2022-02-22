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
    public class ExpedienteDentistaRepository : BaseRepository, IExpedienteDentistaRepository
    {
        public ExpedienteDentistaRepository(IConfiguration iConfiguration) : base(iConfiguration) { }

        public IEnumerable<ExpedienteDentistaAtivosResponse> ListaAtivos()
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("token", token);
                var url = new Uri(urlPadrao + "ExpedientesDentistas/ListaCompletaAtivos/?usuarioId=1");
                var result = http.GetAsync(url).GetAwaiter().GetResult();

                var resultContent = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Erro ao recuperar dentistas. [Msg={result.ReasonPhrase}] [Id=3001]");
                }
                var _retorno = JsonConvert.DeserializeObject<IEnumerable<ExpedienteDentistaAtivosResponse>>(resultContent);

                return _retorno;
            }
            catch
            {
                throw;
            };
        }
    }
}
