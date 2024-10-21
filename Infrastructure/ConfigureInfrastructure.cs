using AutoMapper;
using Domain.Interface.Persistence;
using Domain.Interface.Repository;
using Infrastructure.Persistence;
using Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Infrastructure.Config;

namespace Infrastructure
{
    public static class ConfigureInfrastructure
    {
        public static IServiceCollection AddInfrastructureRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICuentaRepository, CuentaRepository>();
            services.AddSingleton<IMovimientoRepository, MovimientoRepository>();
            services.AddSingleton<IReporteRepository, ReporteRepository>();
            services.AddSingleton<IDomainDbContextFactory, DomainDbContextFactory>();

            services.Configure<Service1Config>(configuration.GetSection("Service1Settings"));

            return services;
        }
}
}
