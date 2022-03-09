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
    public class ListaSpecialtiesController : BaseController
    {
        private readonly IListaSpecialtyService _iService;
        public ListaSpecialtiesController(IListaSpecialtyService iService, IConfiguration iConfiguration) : base(iConfiguration)
        {
            _iService = iService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialtyCompleteResponse>>> Executa()
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

