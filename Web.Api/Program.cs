using Application;
using Application.Services;
using Infrastructure;
using MassTransit;
using MassTransit.Definition;
using System.Reflection;
using Web.Api.Consumers;
using Web.Api.Middleware;
using Web.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CuentaService>();
builder.Services.AddSingleton<MovimientoService>();
builder.Services.AddSingleton<ReporteService>();
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
builder.Services.AddApplicationRegistrations();
builder.Services.AddInfrastructureRegistrations(configuration: builder.Configuration);

/*builder.Services.AddMassTransit(x => {
    x.AddConsumers(Assembly.GetEntryAssembly());
    x.UsingRabbitMq((context, configurator) =>
    {
        var service1Settings = builder.Configuration.GetSection(nameof(Service1Settings)).Get<Service1Settings>();
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMQSettings.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(service1Settings.BaseUrl, false));
        configurator.ReceiveEndpoint("my_query", e =>
        {
            e.Consumer<ClienteConsumer>();
        });
    });
});
builder.Services.AddMassTransitHostedService();*/

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();