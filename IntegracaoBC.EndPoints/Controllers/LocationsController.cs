using IntegracaoBC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationsController : BaseController
    {
        private readonly ILocationService _iService;
        public LocationsController(ILocationService iService)
        {
            _iService = iService;
        }

        [HttpPut("Sync")]
        public async Task<ActionResult> Sync()
        {
            var _erros = (List<string>)await _iService.Sync();

            return _erros.Count > 0 ? BadRequest(_erros) : NoContent();
        }

        [HttpPut("ClearAll")]
        public async Task<ActionResult> ClearAll()
        {
            var _erros = (List<string>)await _iService.ClearAll();

            return _erros.Count > 0 ? BadRequest(_erros) : NoContent();
        }
    }
}
