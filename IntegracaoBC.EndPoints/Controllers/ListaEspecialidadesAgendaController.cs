using IntegracaoBC.Domain.Mappings;
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
    public class ListaEspecialidadesAgendaController : BaseController
    {
        private readonly IListaEspecialidadeAgendaService _iService;
        public ListaEspecialidadesAgendaController(IListaEspecialidadeAgendaService iService, IConfiguration iConfiguration) : base(iConfiguration)
        {
            _iService = iService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EspecialidadeAgendaResponse>>> Executa()
        {
            string _retorno;
            if ((_retorno = Autentica()) != "OK")
                return BadRequest(_retorno);

            try
            {
                return Ok(await _iService.Executa());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
