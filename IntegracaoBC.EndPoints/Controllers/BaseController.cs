using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        private readonly IConfiguration _iConfiguration;
        public BaseController(IConfiguration _configuration)
        {
            _iConfiguration = _configuration;
        }
        protected string Autentica()
        {
            string token = _iConfiguration.GetSection("Auth-token:Token").Value;
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace(" ", "");
                StringValues tokenHeader = "";
                if (!(Request.Headers.TryGetValue("X-Token", out tokenHeader)))
                    return $"Acesso não autorizado [ErroId=000.0001]";

                if ((string)tokenHeader != token)
                    return $"Acesso não autorizado [ErroId=000.0002]";
            }

            return "OK";
        }
    }
}
