using AutoMapper;
using Dapper;
using Domain.Enumerator;
using Domain.Interface.Persistence;
using Domain.Interface.Repository;
using Domain.Entity;
using Infrastructure.Persistence;
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
    public class ReporteRepository : IReporteRepository
    {
        private readonly IDomainDbContextFactory plantContextFactory;

        public ReporteRepository(
          IDomainDbContextFactory plantContextFactory)
        {
            this.plantContextFactory = plantContextFactory;
        }

        public async Task<List<EstadoCuenta>> GetEstadoCuentaAsync(DateTime fechaInicio, DateTime fechaFin, Guid codCliente)
        {
            using var context = await this.plantContextFactory.Create("");
            using (var connection = context.LoadConnection())
            {
                var spName = $"[dbo].[{Enum.GetName(typeof(StoreProcedures), StoreProcedures.sp_GetEstadoCuenta)}]";
                var parameters = new DynamicParameters();
                parameters.Add("fechaIni", fechaInicio, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("fechaFin", fechaFin, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("codCliente", codCliente, System.Data.DbType.Guid, ParameterDirection.Input);

                var results = await connection.QueryMultipleAsync
                     (spName, parameters, commandType: CommandType.StoredProcedure);

                var estadoCuentas = results.Read<EstadoCuenta>().ToList();

                
                return estadoCuentas;
            }
        }
    }
}
