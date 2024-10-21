using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface ICuentaRepository
    {
        Task<string> AddCuenta(Cuenta dto);
        Task<T> GetClienteByCodClienteAsync<T>(Guid clienteId);
        Task<Cuenta?> GetCuentaByNroCtaAsync(string nroCuenta);
        Task<string> UpdateCuenta(Cuenta dto);
        Task<string> DeleteCuenta(Cuenta dto);
    }
}
