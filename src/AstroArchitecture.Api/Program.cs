using AstroCqrs;
using AstroArchitecture.Handlers;
using Serilog;
using AstroArchitecture.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .WriteTo.Console();
});

builder.Services.AddAstroCqrs();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddScoped<IHandlerContext, HandlerContext>();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IDbContext, ApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

var app = builder.Build();

app.MapGetHandler<ListOrders.Query, ListOrders.Response>("/orders.list");
app.MapGetHandler<GetOrderById.Query, GetOrderById.Response>("/orders.getById.{id}");
app.MapGetHandler<GetOrderByCustomerId.Query, GetOrderByCustomerId.Response>("/orders.getOrderByCustomerId.{id}");

app.MapGetHandler<ListCustomers.Query, ListCustomers.Response>("/customers.list");

//app.MapPostHandler<CreateOrder.Command, CreateOrder.Response>("/orders.create");

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();