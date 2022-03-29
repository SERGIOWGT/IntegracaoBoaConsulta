using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Infra.Dental021.Repositories;
using IntegracaoBC.Providers.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.Domain_Test
{
    [TestClass]
    public class ConsultorioRepository_Test
    {
        private IConfiguration  iConfiguration;
        private Provider021Dental iProvider;
        private ConsultorioRepository iRepo;


        public ConsultorioRepository_Test()
        {
            var _myConfiguration = new Dictionary<string, string>();
            _myConfiguration.Add("ConfigApiDental021:UrlBase", "https://dentalapi.homolog.055dental.com.br/api/v1/");
            _myConfiguration.Add("ConfigApiDental021:Token", "H6b6b8f72c02fad5edd.c7pjv99t9v32");

            iConfiguration = new ConfigurationBuilder().AddInMemoryCollection(_myConfiguration).Build();
            iProvider = new(iConfiguration);
            iRepo = new(iProvider);
        }

        [TestMethod]
        public async Task GetAll_Teste()
        {
        
            var _resp = (System.Collections.ICollection) await iRepo.GetAll();
            Assert.IsNotNull(_resp);
            Assert.IsTrue(_resp.Count > 0);
            CollectionAssert.AllItemsAreInstancesOfType(_resp,  typeof(ConsultorioResponse));
        }


        [DataTestMethod]
        [DataRow(0, false)]
        [DataRow(63371, true)]
        public async Task PegaCidade_Teste(long id, bool idEncontrado)
        {
            var _resp = await iRepo.PegaCidade(id);

            if (!idEncontrado)
            {
                Assert.IsNull(_resp);
            } else
            {

                Assert.IsNotNull(_resp);
                Assert.IsInstanceOfType(_resp, typeof(CidadeResponse));
                Assert.IsTrue(_resp.Id == id);
            }
        }
    }
}
