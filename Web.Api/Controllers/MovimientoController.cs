using Application.Dto;
using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("/Movimientos")]
    public class MovimientoController : Controller
    {
        private readonly MovimientoService _movimientoService;
        private readonly IValidator<MovimientoDto> _validator;
        public MovimientoController(MovimientoService movimientoService, IValidator<MovimientoDto> validator)
        {
            this._movimientoService = movimientoService;
            this._validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddMovimientoAsync([FromBody] MovimientoDto dto)
        {
            var result = _validator.Validate(dto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var data = await _movimientoService.AddMovimiento(dto);
            if (data.IsFailed)
                return BadRequest(data);
            else
                return Ok(data);
        }
        [HttpDelete]
        [Route("{codMovimiento}")]
        public async Task<ActionResult<ApiResponse>> DeleteMovimientoAsync(
            [FromRoute] string codMovimiento)
        {
            var data = await _movimientoService.DeleteMovimiento(codMovimiento);
            if (data.IsFailed)
                return BadRequest(data);
            else
                return Ok(data);
        }
    }
}
