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
    public class SpecialtiesController : BaseController
    {
        private readonly ISpecialtyService _iService;
        public SpecialtiesController(ISpecialtyService iService)
        {
            _iService = iService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpecialtyCompleteResponse>>> GetAll()
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

