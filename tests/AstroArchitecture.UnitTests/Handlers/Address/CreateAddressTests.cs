using AstroArchitecture.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Addresses.Commands;
using AstroArchitecture.Handlers.Handlers.Addresses.Events;
using AstroArchitecture.Handlers.Handlers.Addresses.Events.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MinimalCqrs;
using Serilog;

namespace AstroArchitecture.UnitTests.Handlers.Orders;

public class CreateAddressTests
{
    private ServiceProvider ConfigureServices(out ApplicationDbContext dbContext)
    {
        var services = new ServiceCollection();

        services.AddMemoryCache();

        dbContext = InMemoryDbContextFactory.Create();
        services.AddSingleton<IDbContext>(dbContext);
        services.AddSingleton(dbContext);

        services.AddSingleton<ILogger>(new LoggerConfiguration().CreateLogger());

        services.AddScoped<IHandlerContext>(sp =>
            new HandlerContext(
                sp.GetRequiredService<IDbContext>(),
                sp.GetRequiredService<ILogger>()
            ));

        services.AddScoped<CreateAddress.Handler>();
        services.AddScoped<AddressCreated.Handler>();

        services.AddMinimalCqrs();

        return services.BuildServiceProvider();
    }

    private static void SetTestData(IDbContext dc)
    {
        var customer = new Customer("Doom", "Slayer", "doomslayer@fortress-of-doom.com");

        dc.Customers.AddRange(customer);
        dc.SaveChangesAsync();
    }

    private static class CacheKeys
    {
        public static string CustomerAddress(Guid customerId, Guid addressId) => $"{customerId}-{addressId}-{nameof(CustomerAddress)}";
    }

    [Fact]
    public async Task Create_Address_PublishEvent()
    {
        var serviceProvider = ConfigureServices(out var dc);
        SetTestData(dc);

        var addressText = "Fortress of Doom";

        var customer = dc.Customers.First();

        var command = new CreateAddress.Command(customer.Id, addressText, addressText, addressText, addressText, addressText);

        var handler = serviceProvider.GetRequiredService<CreateAddress.Handler>();

        var response = await handler.ExecuteAsync(command, default);

        //var cacheKey = CacheKeys.CustomerAddress(customer.Id, customer.Addresses.First().Id);

        //var customerAddress = serviceProvider.GetRequiredService<IMemoryCache>().Get<AddressReadModel>(cacheKey);

        //Assert.NotNull(customerAddress);
    }
}