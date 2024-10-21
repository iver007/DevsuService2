using Application.Dto;
using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("/Reporte")]
    public class ReporteController : Controller
    {
        private readonly ReporteService _reporteService;
        private readonly IValidator<ReporteDto> _validator;

        public ReporteController(ReporteService reporteService, IValidator<ReporteDto> validator)
        {
            this._reporteService = reporteService;
            this._validator = validator;
        }

        [HttpGet]
        [Route("{fechaInicio}/{fechaFin}/{codCliente}")]
        public async Task<ActionResult<ApiResponse>> GetEstadoCuentaAsync([FromRoute] string fechaInicio,
            [FromRoute] string fechaFin, [FromRoute] Guid codCliente)
        {
            ReporteDto a = new ReporteDto();
            a.FechaInicio = fechaInicio;
            a.FechaFin = fechaFin;
            a.CodigoCliente = codCliente;
            var result = _validator.Validate(a);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var data = await _reporteService.GetEstadoCuenta(a.FechaInicio, a.FechaFin, codCliente);
            if (data.IsFailed)
                return BadRequest(data);
            else
                return Ok(data);
        }
    }
}
