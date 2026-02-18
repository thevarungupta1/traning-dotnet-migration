using EnterpriseLoggingApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;

var builder = Host.CreateDefaultBuilder(args);

builder.UseSerilog((context, services, configuration) =>
{
    configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext();
});

builder.ConfigureServices((context, services) =>
{
    services.AddScoped<IBusinessService, BusinessService>();
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IBusinessService>();

try
{
    using(LogContext.PushProperty("CorrelationId", Guid.NewGuid()))
    {
        await service.ExecuteAsync();
    }
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectly");
}
finally
{
    Log.CloseAndFlush();
}