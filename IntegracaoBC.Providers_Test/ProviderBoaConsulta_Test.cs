using IntegracaoBC.Providers.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBC.Providers_Test
{
    [TestClass]
    public class ProviderBoaConsulta_Test
    {
        private IConfiguration _config;
        private ProviderBoaConsulta _provider;

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

        public ProviderBoaConsulta_Test()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        [DataTestMethod]
        [Priority(0)]
        [TestCategory("Config Error")]
        [DataRow("ConfigApiBoaConsulta:Nada", "Url padrão não informada. [ProviderBC]")]
        [DataRow("ConfigApiBoaConsulta:UrlBase", "ClientId não informado.")]
        [DataRow("ConfigApiBoaConsulta:UrlBase;ConfigApiBoaConsulta:ClientId", "ClientSecret não informado.")]
        [DataRow("ConfigApiBoaConsulta:UrlBase;ConfigApiBoaConsulta:ClientId;ConfigApiBoaConsulta:ClientSecret", "UserId não informado.")]
        public async Task CallAsync_ConfigErro(string chaveInsert, string erro)
        {
            var _myConfiguration = new Dictionary<string, string>();
            var itens = chaveInsert.Split(";");
            foreach (var item in itens)
            {
                _myConfiguration.Add(item, "conteudo");
            }
                        

            _provider = new(new ConfigurationBuilder().AddInMemoryCollection(_myConfiguration).Build());
            
            var _respGet = await _provider.GetAsync("");
            var _respPut = await _provider.PutAsync("", "");
            var _respPost = await _provider.PostAsync("", "");
            var _respDelete = await _provider.DeleteAsync("");


            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.Resultado == erro);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.Resultado == erro);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.Resultado == erro);

            Assert.IsFalse(_respDelete.Sucesso);
            Assert.IsTrue(_respDelete.Resultado == erro);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Call Error")]
        public async Task CallAsync_UrlNullOrEmpty()
        {

            _provider = new(Configuration);
            var erro = "Url não informada. [ProviderBC]";

            var _respGet = await _provider.GetAsync("");
            var _respPut = await _provider.PutAsync("", "");
            var _respPost = await _provider.PostAsync("", "");
            var _respDelete = await _provider.DeleteAsync("");

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.Resultado == erro);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.Resultado == erro);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.Resultado == erro);

            Assert.IsFalse(_respDelete.Sucesso);
            Assert.IsTrue(_respDelete.Resultado == erro);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("Call Error")]
        public async Task GetAsync_UrlInexistente()
        {

            _provider = new(Configuration);

            var _respGet = await _provider.GetAsync("xxx");
            var _respPut = await _provider.PutAsync("", "xxx");
            var _respPost = await _provider.PostAsync("", "xxx");
            var _respDelete = await _provider.DeleteAsync("xxx");

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respGet.CodigoHttp == System.Net.HttpStatusCode.NotFound);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.NotFound);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.CodigoHttp == System.Net.HttpStatusCode.NotFound);

            Assert.IsFalse(_respDelete.Sucesso);
            Assert.IsTrue(_respDelete.CodigoHttp == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Call Error")]
        public async Task CallAsync_MetodoNaoPermito()
        {

            _provider = new(Configuration);

            var _respGet = await _provider.GetAsync("agenda/marca");
            var _respPut = await _provider.PutAsync("", "agenda/ListaHorariosLivresBoaConsulta?");
            var _respPost = await _provider.PostAsync("", "agenda/marca");
            var _respDelete = await _provider.DeleteAsync("agenda/marca");

            Assert.IsFalse(_respGet.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);

            Assert.IsFalse(_respPut.Sucesso);
            Assert.IsTrue(_respPut.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);

            Assert.IsFalse(_respPost.Sucesso);
            Assert.IsTrue(_respPost.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);

            Assert.IsFalse(_respDelete.Sucesso);
            Assert.IsTrue(_respDelete.CodigoHttp == System.Net.HttpStatusCode.MethodNotAllowed);
        }


        [TestMethod]
        [Priority(1)]
        [TestCategory("Call Error")]
        public async Task PostPutAsync_ComErro()
        {

            _provider = new(Configuration); ;
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
