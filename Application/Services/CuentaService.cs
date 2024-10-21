using Application.Dto;
using AutoMapper;
using Domain.Enumerator;
using Domain.Interface.Repository;
using Domain.Entity;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Application.Services
{
    public class CuentaService
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly IMapper _mapper;

        public CuentaService(ICuentaRepository cuentaRepository, IMovimientoRepository movimientoRepository,
            IMapper mapper)
        {
            this._cuentaRepository = cuentaRepository;
            this._movimientoRepository = movimientoRepository;
            this._mapper = mapper;
        }
        public async Task<ApiResponse> AddCuenta(CuentaAddDto dto)
        {
            var data = await this._cuentaRepository.GetClienteByCodClienteAsync<ApiResponse>(dto.CodigoCliente);
            if (data.Data != null)
            {
                Cliente cliente = JsonConvert.DeserializeObject<Cliente>(data.Data.ToString());
                if (cliente.Estado)
                {
                    Cuenta obj = _mapper.Map<Cuenta>(dto);
                    obj.NumeroCuenta = DateTime.Now.ToString("yyyyMMddHHmmss");
                    obj.ClientesId = cliente.Id;
                    obj.SaldoActual = Math.Round(dto.SaldoInicial, 2);

                    var result = await _cuentaRepository.AddCuenta(obj);

                    Movimiento mov = new Movimiento(DateTime.Now, Movimientos.Inicial, Math.Round(dto.SaldoInicial, 2),
                        Math.Round(dto.SaldoInicial, 2), true, result, DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString()) ;
                    await _movimientoRepository.AddMovimiento(mov);

                    return ActionResultApiResponse("Ok", result, false);
                }
                return ActionResultApiResponse("Cliente inactive", dto, true);
            }
            return ActionResultApiResponse("No exists Cliente", dto, true);
        }
        public async Task<ApiResponse> UpdateCuenta(string nroCta, CuentaUpdateDto dto)
        {
            var cuenta = await this._cuentaRepository.GetCuentaByNroCtaAsync(nroCta);
            if (cuenta != null)
            {
                cuenta.TipoCuenta = (Cuentas)Enum.Parse(typeof(Cuentas), (dto.TipoCuenta??""));
                cuenta.Estado = dto.Estado;

                await _cuentaRepository.UpdateCuenta(cuenta);
                return ActionResultApiResponse("Ok", cuenta, false);

            }
            return ActionResultApiResponse("No exists NumeroCuenta", dto, true);
        }
        public async Task<ApiResponse> DeleteCuenta(string nroCta)
        {
            var obj = await this._cuentaRepository.GetCuentaByNroCtaAsync(nroCta.Trim());
            if (obj != null)
            {
                var result = await _cuentaRepository.DeleteCuenta(obj);
                return ActionResultApiResponse("Ok", obj, false);
            }

            return ActionResultApiResponse("No exists NumeroCuenta", obj, true);
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
