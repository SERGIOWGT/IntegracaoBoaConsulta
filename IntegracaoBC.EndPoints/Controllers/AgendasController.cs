/*
using IntegracaoBC.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgendasController : BaseController
    {
        private readonly IAgendaService _iService;
        public AgendasController(IAgendaService iService)
        {
            _iService = iService;
        }

        [HttpPut("Sync")]
        public async Task<ActionResult> Sync()
        {
            var _erros = (List<string>)await _iService.Sync();

            return _erros.Count > 0 ? BadRequest(_erros) : NoContent();
        }
    }
}
*/