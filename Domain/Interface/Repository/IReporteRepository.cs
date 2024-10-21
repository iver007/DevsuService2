using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IReporteRepository
    {
        Task<List<EstadoCuenta>> GetEstadoCuentaAsync(DateTime fechaInicio, DateTime fechaFin, Guid codCliente);
    }
}
