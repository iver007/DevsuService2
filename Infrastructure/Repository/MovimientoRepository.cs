using AutoMapper;
using Dapper;
using Domain.Enumerator;
using Domain.Interface.Persistence;
using Domain.Interface.Repository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly IDomainDbContextFactory plantContextFactory;

        public MovimientoRepository(
          IDomainDbContextFactory plantContextFactory)
        {
            this.plantContextFactory = plantContextFactory;
        }

        public async Task<Movimiento?> GetMovimientoActiveByNroCtaAsync(string nroCuenta)
        {
            using (var context = await this.plantContextFactory.Create(""))
            {
                var movimiento = await context.Movimientos.Where(x => x.Cuentas.NumeroCuenta == nroCuenta && x.Estado).FirstOrDefaultAsync();
                return movimiento;
            }
        }

        public async Task<Movimiento?> GetMovimientoByCodMovimientoAsync(string codMovimiento)
        {
            using (var context = await this.plantContextFactory.Create(""))
            {
                var movimiento = await context.Movimientos.Where(x => x.CodMovimiento == codMovimiento).FirstOrDefaultAsync();
                return movimiento;
            }
        }

        public async Task<string> UpdateOffMovimiento(Movimiento? dto)
        {
            if(dto != null)
            {
                using (var context = await this.plantContextFactory.Create(""))
                {
                    dto.Estado = false;
                    context.Movimientos.Update(dto);
                    await context.SaveAsync();
                    return "";
                }
            }
            return "";
        }
        public async Task<string> AddMovimiento(Movimiento dto)
        {           
            using (var context = await this.plantContextFactory.Create(""))
            {
                context.Movimientos.Add(dto);
                await context.SaveAsync();
                return dto.NumeroCuenta;
            }
        }
    }
}
