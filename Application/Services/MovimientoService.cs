using Application.Dto;
using AutoMapper;
using Domain.Enumerator;
using Domain.Interface.Repository;
using Domain.Entity;
using System.Drawing;

namespace Application.Services
{
    public class MovimientoService
    {
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IMapper _mapper;

        public MovimientoService(IMovimientoRepository movimientoRepository, ICuentaRepository cuentaRepository, IMapper mapper)
        {
            this._movimientoRepository = movimientoRepository;
            this._cuentaRepository = cuentaRepository;
            this._mapper = mapper;
        }
        public async Task<ApiResponse> AddMovimiento(MovimientoDto dto)
        {
            var cuenta = await this._cuentaRepository.GetCuentaByNroCtaAsync(dto.NumeroCuenta ?? "");
            if (cuenta != null)
            {
                var movimientoActive = await this._movimientoRepository.GetMovimientoActiveByNroCtaAsync(dto.NumeroCuenta ?? "");
               
                double currentBalance = movimientoActive != null ? movimientoActive.Saldo : cuenta.SaldoActual;
                if (currentBalance + dto.Valor >= 0) {
                    cuenta.SaldoActual = Math.Round(currentBalance + dto.Valor, 2);

                    await this._cuentaRepository.UpdateCuenta(cuenta);
                    await this._movimientoRepository.UpdateOffMovimiento(movimientoActive);

                    Movimiento obj = new Movimiento(DateTime.Now, dto.Valor > 0 ? Movimientos.Ingreso : Movimientos.Egreso, dto.Valor
                        , Math.Round(currentBalance + dto.Valor, 2), true, cuenta.NumeroCuenta, DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString());
                    await _movimientoRepository.AddMovimiento(obj);
                    return ActionResultApiResponse("Ok", obj, false);
                }
                return ActionResultApiResponse("balance not available", dto, true);
            }
            return ActionResultApiResponse("No exists Cuenta", dto, true);
        }
        public async Task<ApiResponse> DeleteMovimiento(string codMovimiento)
        {
            var movimiento = await this._movimientoRepository.GetMovimientoByCodMovimientoAsync(codMovimiento.Trim());
            if (movimiento != null && movimiento.TipoMovimiento != Movimientos.Inicial)
            {
                var movimientoActive = await this._movimientoRepository.GetMovimientoActiveByNroCtaAsync(movimiento.NumeroCuenta);
                var cuenta = await this._cuentaRepository.GetCuentaByNroCtaAsync(movimiento.NumeroCuenta);
                
                double currentBalance = movimientoActive != null ? movimientoActive.Saldo : cuenta.SaldoActual;
                if (currentBalance - movimiento.Valor >= 0) {
                    cuenta.SaldoActual = Math.Round(currentBalance - movimiento.Valor, 2);

                    await this._cuentaRepository.UpdateCuenta(cuenta);
                    await this._movimientoRepository.UpdateOffMovimiento(movimientoActive);
                    await this._movimientoRepository.UpdateOffMovimiento(movimiento);

                    Movimiento obj = new Movimiento(DateTime.Now, -movimiento.Valor > 0 ? Movimientos.Ingreso : Movimientos.Egreso
                        , -movimiento.Valor, Math.Round(currentBalance - movimiento.Valor, 2), true,
                         movimiento.NumeroCuenta, DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString());
                    await _movimientoRepository.AddMovimiento(obj);
                    return ActionResultApiResponse("Ok", obj, false);
                }
                return ActionResultApiResponse("balance not available", movimiento, true);
            }

            return ActionResultApiResponse("No exists or it is an initial Movimiento", movimiento, true);
        }
        private ApiResponse ActionResultApiResponse(string message, object? data = null, bool IsFailed = true)
        {
            return new ApiResponse
            {
                IsFailed = IsFailed,
                Message = message,
                Data = data
            };
        }
    }
}
