using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EspecialidadesAgendaController : BaseController
    {
        private readonly IEspecialidadeAgendaService _iService;
        public EspecialidadesAgendaController(IEspecialidadeAgendaService iService)
        {
            _iService = iService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EspecialidadeAgendaResponse>>> GetAll()
        {
            try
            {
                return Ok(await _iService.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
