using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ListaVagasController : BaseController
    {

        private readonly ILoggerFactory _iLoggerFactory;
        private readonly IListaVagasService _iService;
        public ListaVagasController(IListaVagasService iService, IConfiguration iConfiguration, ILoggerFactory iLoggerFactory) : base(iConfiguration)
        {
            _iService = iService;
            _iLoggerFactory = iLoggerFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListaVagasResponse>>> Executa([FromQuery] ListaVagasRequest request)
        {
            var logger = _iLoggerFactory.CreateLogger<ListaVagasController>();

            string _retorno = $"{Request.Method} ==> {Request.Path} {Environment.NewLine}";
            _retorno += "==> QUERYSTRING: " + Environment.NewLine + Request.QueryString.Value + Environment.NewLine;
            _retorno += "==> HEADER: " + Environment.NewLine;
            foreach (var info in Request.Headers)
            {
                _retorno += $"| {info.Key} = {info.Value}";
            }
            logger.LogInformation(_retorno);
                        
            if ((_retorno = Autentica()) != "OK")
            {
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }                

            var expedienteDentista = request.agenda_id.Split("_");
            if (expedienteDentista.Length != 3)
            {
                _retorno = $"agenda_id inválido. [Id={request.agenda_id}]";
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }                

            if (expedienteDentista[2] != request.doctor_id)
            {
                _retorno = $"doctor_id inválido. [Id={request.doctor_id}]";
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }
                

            long expedienteId = 0;
            if (long.TryParse(expedienteDentista[0], out expedienteId) == false)
            {
                _retorno = $"ExpedienteId inválido. [Id={expedienteDentista[0]}]";
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }                

            long dentistaId = 0;
            if (long.TryParse(expedienteDentista[2], out dentistaId) == false)
            {
                _retorno = $"DentistaId inválido. [Id={expedienteDentista[2]}]";
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }                

            DateTime dataInicio;
            if (DateTime.TryParse(request.start_date, out dataInicio) == false)
            {
                _retorno = $"start_date inválido. [value={request.start_date}]";
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }                

            DateTime dataFim;
            if (DateTime.TryParse(request.end_date, out dataFim) == false)
            {
                _retorno = $"end_date inválido. [value={request.end_date}]";
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }
                
            try
            {
                return Ok(await _iService.Executa(request.agenda_id, expedienteId, dentistaId, dataInicio, dataFim));
            }
            catch (Exception e)
            {
                _retorno = e.Message;
                logger.LogError(_retorno);
                return BadRequest(_retorno);
            }
        }
    }
}
