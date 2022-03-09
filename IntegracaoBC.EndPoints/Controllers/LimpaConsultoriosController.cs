using IntegracaoBC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LimpaConsultoriosController : BaseController
    {
        private readonly ILimpaConsultoriosService _iService;
        public LimpaConsultoriosController(ILimpaConsultoriosService iService, IConfiguration iConfiguration) : base(iConfiguration)
        {
            _iService = iService;
        }

        [HttpPut("")]
        public async Task<ActionResult> Executa()
        {
            string _retorno;
            if ((_retorno = Autentica()) != "OK")
                return BadRequest(_retorno);

            try
            {
                var _erros = (List<string>)await _iService.Executa();

                return _erros.Count > 0 ? BadRequest(_erros) : NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
