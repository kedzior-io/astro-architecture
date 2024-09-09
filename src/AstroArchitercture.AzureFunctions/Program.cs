using AstroArchitecture.Handlers;
using AstroArchitecture.Infrastructure.Persistence;
using AstroArchitercture.AzureFunctions;
using Azure.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.WithProperty("SourceLogger", "Serilog");

var logger = loggerConfiguration.CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.Serializer = new JsonObjectSerializer(CustomJsonOptions.Defaults);
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<ILogger>(_ => logger);
        services.AddDbContext<IDbContext, ApplicationDbContext>();
        services.AddScoped<IHandlerContext, AzureFunctionsHandlerContext>();

        // TODO: Looking for a better way to find and register handlers
        services.AddAstroCqrsFromAssemblyContaining<ListOrders.Query>();

        foreach (var service in services)
        {
            Console.WriteLine($"Service: {service.ServiceType.FullName}, Lifetime: {service.Lifetime}");
        }
    })
    .Build();

host.Run();