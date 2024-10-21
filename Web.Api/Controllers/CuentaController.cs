using Application.Dto;
using Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [Route("/Cuentas")]
    public class CuentaController : Controller
    {
        private readonly CuentaService _cuentaService;
        private readonly IValidator<CuentaAddDto> _validator;
        private readonly IValidator<CuentaUpdateDto> _validatorCuentaUpdate;
        public CuentaController(CuentaService cuentaService, IValidator<CuentaAddDto> validator, IValidator<CuentaUpdateDto> validatorCuentaUpdate)
        {
            this._cuentaService = cuentaService;
            this._validator = validator;
            this._validatorCuentaUpdate = validatorCuentaUpdate;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddCuentaAsync([FromBody] CuentaAddDto dto)
        {
            var result = _validator.Validate(dto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var data = await _cuentaService.AddCuenta(dto);
            if (data.IsFailed)
                return BadRequest(data);
            else
                return Ok(data);
        }
        [HttpPut]
        [Route("{nroCta}")]
        public async Task<ActionResult<ApiResponse>> UpdateClienteAsync(
            [FromRoute] string nroCta, [FromBody] CuentaUpdateDto dto)
        {
            var result = _validatorCuentaUpdate.Validate(dto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var data = await _cuentaService.UpdateCuenta(nroCta, dto);
            if (data.IsFailed)
                return BadRequest(data);
            else
                return Ok(data);
        }
        [HttpDelete]
        [Route("{nroCta}")]
        public async Task<ActionResult<ApiResponse>> DeleteCuentaAsync(
             [FromRoute] string nroCta)
        {
            var data = await _cuentaService.DeleteCuenta(nroCta);
            if (data.IsFailed)
                return BadRequest(data);
            else
                return Ok(data);
        }
    }
}
