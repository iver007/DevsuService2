using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IMovimientoRepository
    {
        Task<Movimiento?> GetMovimientoActiveByNroCtaAsync(string nroCuenta);
        Task<Movimiento?> GetMovimientoByCodMovimientoAsync(string codMovimiento);
        Task<string> AddMovimiento(Movimiento dto);
        Task<string> UpdateOffMovimiento(Movimiento? dto);
    }
}
