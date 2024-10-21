using Application.Dto;
using AutoMapper;
using Domain.Enumerator;
using Domain.Interface.Repository;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Application.Services
{
    public class ReporteService
    {
        private readonly IReporteRepository _reporteRepository;
        private readonly IMapper _mapper;

        public ReporteService(IReporteRepository reporteRepository, IMapper mapper)
        {
            this._reporteRepository = reporteRepository;
            this._mapper = mapper;
        }
        public async Task<ApiResponse> GetEstadoCuenta(string fechaInicio, string fechaFin, Guid codCliente)
        {
            var result = await this._reporteRepository.GetEstadoCuentaAsync(
                DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                DateTime.ParseExact(fechaFin, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), 
                codCliente);
            return ActionResultApiResponse("ok", result, false);
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
