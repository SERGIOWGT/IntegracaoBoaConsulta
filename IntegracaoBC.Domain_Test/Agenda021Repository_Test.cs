using IntegracaoBC.Domain.Implementations;
using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Providers.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain_Test
{
    [TestClass]
    public class Agenda021Repository_Test
    {
        private IConfiguration iConfiguration;
        private ProviderAgenda021 iProvider;
        private Agenda021Repository iRepo;


        public Agenda021Repository_Test()
        {
            var _myConfiguration = new Dictionary<string, string>();
            _myConfiguration.Add("ConfigApiAgenda021:UrlBase", "https://agenda.api.homolog.021dental.com.br/api/");
            _myConfiguration.Add("ConfigApiAgenda021:Token", "H6b6b8f72c02fad5edd.c7pjv99t9v32");

            iConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_myConfiguration).Build();
            iProvider = new(iConfiguration);
            iRepo = new(iProvider);
        }


        /*
        public async Task<Tuple<string, long>> MarcaConsulta(MarcaConsultaRequest novo)
        public async Task<IEnumerable<DataSlotResponse>> ListaVagas(long expedienteId, long especialidadeAgendaId, long consultorioId, DateTime dataInicio, DateTime dataFim)
        */

        [DataTestMethod]
        [DataRow(0, 0, 0, null, null)]
        [DataRow(0, 0, 0, null, null)]
        [DataRow(0, 0, 0, null, null)]
        [DataRow(0, 0, 0, null, null)]
        [DataRow(0, 0, 0, null, null)]
        [DataRow(0, 0, 0, null, null)]
        public async Task ListaVagas_ParametrosInvalidos(long expedienteId, long especialidadeAgendaId, long consultorioId, DateTime? dataInicio, DateTime? dataFim)
        {
            DateTime _dataInicio = (DateTime)((dataInicio == null) ? DateTime.Now : dataInicio);
            DateTime _dataFim = (DateTime)((dataFim == null) ? DateTime.Now : dataFim);

            var _resp = await iRepo.ListaVagas(expedienteId, especialidadeAgendaId, consultorioId, _dataInicio, _dataFim);


        }

    }
}
