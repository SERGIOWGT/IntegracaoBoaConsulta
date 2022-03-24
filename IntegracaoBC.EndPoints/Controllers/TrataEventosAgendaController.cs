using IntegracaoBC.Domain.Mappings;
using IntegracaoBC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IntegracaoBC.EndPoints.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TrataEventosAgendaController : BaseController
    {

        private readonly ITrataEventosAgendaService _iService;
        private readonly ILoggerFactory _iLoggerFactory;
        public TrataEventosAgendaController(ITrataEventosAgendaService iService, IConfiguration iConfiguration, ILoggerFactory iLoggerFactory) : base(iConfiguration)
        {
            _iService = iService;
            _iLoggerFactory = iLoggerFactory;
        }

        [HttpPost("")]
        public async Task<ActionResult> Executa(AppointmentEventRequest request)
        {
            var logger = _iLoggerFactory.CreateLogger<TrataEventosAgendaController>();

            long _agendamentoId = 0;
            string _retorno = $"{Request.Method} ==> {Request.Path} {Environment.NewLine}";

            _retorno += $"==> [BCId={request.data.id}]" + Environment.NewLine;
            _retorno += "==> HEADER: " + Environment.NewLine;
            foreach (var info in Request.Headers)
            {
                _retorno += $"| {info.Key} = {info.Value}";
            }
            _retorno += Environment.NewLine;
            _retorno += "==> BODY: " + Environment.NewLine;
            _retorno += JsonConvert.SerializeObject(request);
            _retorno += Environment.NewLine;
            logger.LogInformation(_retorno);

            try
            {
                (_retorno, _agendamentoId) = await _iService.Executa(request);
                if (_retorno != "OK")
                {
                    logger.LogError($"[BCId={request.data.id}] ==> {_retorno}");
                    return BadRequest(_retorno);
                }
                logger.LogInformation($"Agendamento Criado na  [agendamentoId={_agendamentoId}] [BCId={request.data.id}]");

                (_retorno, _) = await _iService.Confirma(request.data.id);
                if (_retorno != "OK")
                {
                    logger.LogError($"[BCId={request.data.id}] ==> {_retorno}");
                    return BadRequest(_retorno);
                }

                logger.LogInformation($"Agendamento confirmado no BoaConsulta [agendamentoId={_agendamentoId}] [BCId={request.data.id}]");
                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}/confirma")]
        public async Task<ActionResult> Confirma(string id)
        {
            try
            {
                var (_retorno, _retorno2) = await _iService.Confirma(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
