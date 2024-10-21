using AutoMapper;
using Domain.Enumerator;
using Domain.Interface.Persistence;
using Domain.Interface.Repository;
using Domain.Entity;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace Infrastructure.Repository
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly IDomainDbContextFactory plantContextFactory;
        private readonly string service1ApiUrl;
        public CuentaRepository(
          IDomainDbContextFactory plantContextFactory,
           IOptions<Service1Config> options)
        {
            this.plantContextFactory = plantContextFactory;
            this.service1ApiUrl = options.Value.BaseUrl;
        }
        public async Task<T> GetClienteByCodClienteAsync<T>(Guid clienteId)
        {
            var result = await Flurl.Url.Combine(this.service1ApiUrl, "/Clientes" + $"/{clienteId}").GetAsync().ReceiveJson<T>().ConfigureAwait(false);
            return result;
        }

        public async Task<string> AddCuenta(Cuenta dto)
        {
            using (var context = await this.plantContextFactory.Create(""))
            {
                context.Cuentas.Add(dto);
                await context.SaveAsync();
                return dto.NumeroCuenta;
            }
        }

        public async Task<Cuenta?> GetCuentaByNroCtaAsync(string nroCuenta)
        {
            using (var context = await this.plantContextFactory.Create(""))
            {
                var cuenta = await context.Cuentas.Where(x => x.NumeroCuenta == nroCuenta).FirstOrDefaultAsync();
                return cuenta;
            }
        }

        public async Task<string> UpdateCuenta(Cuenta dto)
        {
            using (var context = await this.plantContextFactory.Create(""))
            {
                context.Cuentas.Update(dto);
                await context.SaveAsync();
                return "";
            }
        }

        public async Task<string> DeleteCuenta(Cuenta dto)
        {
            using (var context = await this.plantContextFactory.Create(""))
            {
                context.Cuentas.Remove(dto);
                await context.SaveAsync();
                return "";
            }
        }
    }
}
