using Microsoft.Extensions.Configuration;
using IntegracaoBC.Providers.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IntegracaoBC.Providers_Test
{
    [TestClass]
    public class ProviderAgenda021_Test
    {
        private IConfiguration _config;
        
        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }

        public ProviderAgenda021_Test()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Config Error")]
        public async Task CallAsync_SemUrlPadrao()
        {
            var erro = "Url padrão não encontrada.";
            var builder = new ConfigurationBuilder();

            ProviderAgenda021 _provider = new(builder.Build());

            var _respGet = await _provider.GetAsync("");
            var _respPut = await _provider.PutAsync("", "");
            var _respPost = await _provider.PostAsync("", "");

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.Resultado == erro);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.Resultado == erro);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.Resultado == erro);
        }
        [TestMethod]
        [Priority(0)]
        [TestCategory("Config Error")]
        public async Task CallAsync_SemToken()
        {
            var _myConfiguration = new Dictionary<string, string>();
            _myConfiguration.Add("ConfigApiAgenda021:UrlBase", "https://agenda.api.homolog.021dental.com.br/api/");

            ProviderAgenda021 _provider = new(new ConfigurationBuilder().AddInMemoryCollection(_myConfiguration).Build());
            var erro = "Token de segurança não encontrado.";

            var _respGet = await _provider.GetAsync("");
            var _respPut = await _provider.PutAsync("", "");
            var _respPost = await _provider.PostAsync("", "");


            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.Resultado == erro);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.Resultado == erro);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.Resultado == erro);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Call Error")]
        public async Task CallAsync_UrlNullOrEmpty()
        {

            ProviderAgenda021 _provider = new(Configuration);
            var erro = "url não informada.";

            var _respGet = await _provider.GetAsync("");
            var _respPut = await _provider.PutAsync("", "");
            var _respPost = await _provider.PostAsync("", "");

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.Resultado == erro);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.Resultado == erro);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.Resultado == erro);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Call Error")]
        public async Task GetAsync_UrlInexistente()
        {

            ProviderAgenda021 _provider = new(Configuration);

            var _respGet = await _provider.GetAsync("xxx");
            var _respPut = await _provider.PutAsync("", "xxx");
            var _respPost = await _provider.PostAsync("", "xxx");

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.CodigoHttp == System.Net.HttpStatusCode.NotFound);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.NotFound);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.CodigoHttp == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Call Error")]
        public async Task CallAsync_MetodoNaoPermito()
        {

            ProviderAgenda021 _provider = new(Configuration);

            var _respGet = await _provider.GetAsync("agenda/marca");
            var _respPut = await _provider.PutAsync("", "agenda/ListaHorariosLivresBoaConsulta?");
            var _respPost = await _provider.PostAsync("", "agenda/marca"); 

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Call Error")]
        public async Task PostPutAsync_ComErro()
        {

            ProviderAgenda021 _provider = new(Configuration); ;
            var erro = "O Cpf é obrigatório.";

            string jsonParam = "{\"Nome\":\"Nome do Paciente\", \"Sexo\": \"M\"}";

            var _respPut = await _provider.PutAsync(jsonParam, "pacientes/23121");
            var _respPost = await _provider.PostAsync(jsonParam, "pacientes");

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.BadRequest);
            Assert.IsTrue(_respPut.Resultado == erro);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.CodigoHttp == System.Net.HttpStatusCode.BadRequest);
            Assert.IsTrue(_respPost.Resultado == erro);
        }


    }
}
