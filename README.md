﻿# Astro Architecture Solution Template for .NET8

This is a solution template for creating ASP.NET Core Web API that puts together CQRS + DDD (Domain Driven Design) and Vertical Slice architecture.

It uses [AstroCQRS](https://github.com/kedzior-io/astro-cqrs) for zero-setup CQRS and Vertical Slice architecture.

## Motives

I often read on how CQRS and Vertical Architecture is an overkill and has big configuring overhead. This template proves it can be literary zero-setup. It also 

- easy to start / zero-setup
- keep simple structure
- enforce consistency
- testibility
 
## Project structure

- `Api` - thin API layer using Minimal API
- `Handlers` - class library where all query and command handlers live
- `Domain` - all domain entities and domain services
- `Infrastructure` - persistence setup, sending emails etc
- `Core` - common stuff with no major dependecies

## Installation & Running

1. The project creates SQLite database on the runtime, go to [`AstroArchitecture.Core.Constants.ConnectionStrings`](https://github.com/kedzior-io/astro-architecture/blob/main/src/AstroArchitecture.Core/Constants/ConnectionStrings.cs) and set the desired location of your SQLite. Use location the runtime can write to. 

2. Once launched it will pop up swagger page: `/swagger/index.html`

## Usage 

1. Create a GET endpoint

```csharp
app.MapGetHandler<ListOrders.Query, ListOrders.Response>("/orders.list");
```

2. Create a handler 👇

```csharp
public static class ListOrders
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;

    public sealed class Response(IReadOnlyCollection<Order> orders)
    {
        public IReadOnlyCollection<OrderListModel> Orders { get; private set; } = orders.Select(OrderListModel.Create).ToList();
    }

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var orders = await DbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ToListAsync(ct);

            return Success(new Response(orders));
        }
    }
}
```

... and that's it! 🙌

3. Create a POST endpoint 👇

```csharp
app.MapPostHandler<CreateProduct.Command, CreateProduct.Response>("/products.create");
```

4. Create a handler 👇

```csharp
public static class CreateProduct
{
    public sealed record Command(string Name, decimal Price, int Stock) : ICommand<IHandlerResponse<Response>>;
    public sealed record Response(int ProductId);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Stock)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var product = new Product(command.Name, command.Price, command.Stock);
            await DbContext.Products.AddAsync(product, ct);
            await DbContext.SaveChangesAsync(ct);

            return Success(new Response(product.Id));
        }
    }
}
```

... and that's it! 🙌


5. Unit test your handler 👇

```csharp

public class CreateProductTests
{
    private static CreateProduct.Handler GetHandler(ApplicationDbContext dbContext)
    {
        return new CreateProduct.Handler(HandlerContextFactory.GetHandlerContext(dbContext));
    }

    private static void SetTestData(ApplicationDbContext dc)
    {
        var products = new List<Product> {
            new("Cyberpunk 2077", 24.2m, 100),
            new("Dune: Spice Wars", 19.99m, 100),
            new("Starcraft 2: ", 0.99m, 100),
        };

        dc.Products.AddRange(products);
        dc.SaveChanges();
    }

    [Fact]
    public async Task Create_Product_ReturnProductId()
    {
        var dc = InMemoryDbContextFactory.Create();
        SetTestData(dc);

        var productName = "Doom: Eternal";

        var command = new CreateProduct.Command(productName, 6.66m, 666);

        var expectedName = productName;
        var expectedCount = 4;

        var response = await GetHandler(dc).ExecuteAsync(command, default);

        Assert.NotNull(response?.Payload?.ProductId);

        var result = dc.Products.SingleOrDefault(o => o.Id == response.Payload.ProductId);
        var actualCount = dc.Products.Count();

        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
        Assert.Equal(expectedCount, expectedCount);
    }

    [Fact]
    public void Create_Product_ReturnNameTooShort()
    {
        var dc = InMemoryDbContextFactory.Create();
        SetTestData(dc);

        var command = new CreateProduct.Command("", 6.66m, 666);

        Assert.ThrowsAsync<ArgumentException>(async () => await GetHandler(dc).ExecuteAsync(command, default));
    }
}
```

6. Cache Middleware for your endpoints

The Cache Middleware allows you to cache responses for specified endpoints in your ASP.NET Core application, improving performance by reducing the need for repeated processing of identical requests.

### Overview

This middleware leverages the **FusionCacheProvider** to store cached responses for a duration defined in the middleware. It checks if caching is enabled for the endpoint and handles the retrieval and storage of cached responses.


## Features

- **Efficient Response Caching**: Caches HTTP responses for specified endpoints.
- **FusionCache Integration**: Leverages FusionCacheProvider for advanced caching capabilities, including support for memory and distributed caching.
- **Dynamic Cache Key Generation**: Cache keys are generated based on the request URL and query strings. It can be configured in **GenerateCacheKey** function.
- **Configurable Cache Duration**: Responses are cached for 30 minutes by default, but this can be modified as needed.
- **Custom Cache Metadata**: The caching behavior is controlled using endpoint metadata (CacheMetadata).

### Usage


1. **Add the Middleware to the Pipeline**

   You can add the Cache Middleware to your application like this:

   ```csharp
   app.UseMiddleware<CacheMiddleware>();
   ```

2. **Example Endpoint with Caching**

   Here’s how to create a GET endpoint that uses the cache middleware:

   ```csharp
   app.MapGetHandler<ListProducts.Query, ListProducts.Response>("/products.list")
       .UseCache();
   ```
## More usages:

Check samples here: [AstroCQRS](https://github.com/kedzior-io/astro-cqrs)

More info: [Building .NET 8 APIs with Zero-Setup CQRS and Vertical Slice Architecture](https://dev.to/kedzior_io/building-net-8-apis-with-zero-setup-cqrs-and-vertical-slice-architecture-528p)

## Todo

Here are things I want to add to this repo:

- UI example
- Blzor example
- MVC example
- Integration test example
- Benchmarks
  
## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Questions, feature requests

- Create a [new issue](https://github.com/kedzior-io/astro-cqrs/issues/new)
- [Twitter](https://twitter.com/KedziorArtur)
- [Discord](https://discord.gg/j3vmcaZG)
