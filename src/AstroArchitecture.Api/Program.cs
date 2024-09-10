using AstroCqrs;
using Serilog;
using AstroArchitecture.Handlers;
using AstroArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

//using AstroArchitecture.Api.Azure;
//using AstroArchitecture.Infrastructure.Providers.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .WriteTo.Console();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddScoped<IHandlerContext, HandlerContext>();
builder.Services.AddDbContext<IDbContext, ApplicationDbContext>();

// Uncomment to try CacheContext with /customers.cache.list endpoint
// builder.Services.AddSingleton<ICacheContext, CacheProvider>();

// Uncomment to try Service Bus triggered Azure Functions
// builder.Services.AddServiceBus();

builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(s => s.FullName?.Replace("+", ".")); });

builder.Services.AddAstroCqrs();

var app = builder.Build();

/*
 * Add your endpoint here
 */

app.MapGetHandler<ListCustomers.Query, ListCustomers.Response>("/customers.list");
app.MapPostHandler<CreateCustomer.Command, CreateCustomer.Response>("/customers.create");
app.MapPostHandler<UpdateCustomer.Command>("/customers.update");
app.MapGetHandler<ListCachedCustomers.Query, ListCachedCustomers.Response>("/customers.cache.list");

app.MapGetHandler<ListAddresses.Query, ListAddresses.Response>("/addresses.list");
app.MapPostHandler<CreateAddress.Command, CreateAddress.Response>("/addresses.create");

app.MapGetHandler<ListProducts.Query, ListProducts.Response>("/products.list");
app.MapPostHandler<CreateProduct.Command, CreateProduct.Response>("/products.create");

app.MapGetHandler<ListOrders.Query, ListOrders.Response>("/orders.list");
app.MapPostHandler<CreateOrder.Command, CreateOrder.Response>("/orders.create");

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    /*
     * Do not run this in production, not really recommended
     * https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
     */

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();