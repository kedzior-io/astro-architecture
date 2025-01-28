﻿# Astro Architecture Solution Template for .NET 9

This solution template provides a streamlined and modern way to build ASP.NET Core Web APIs by combining CQRS (Command Query Responsibility Segregation), DDD (Domain-Driven Design), and Vertical Slice Architecture. It’s designed to be simple, consistent, and testable while minimizing setup overhead.

Powered by [AstroCQRS](https://github.com/kedzior-io/astro-cqrs), this template delivers a truly zero-setup experience - literally requiring just this single line:

```csharp
builder.Services.AddAstroCqrs();
```

# Why use this template?

Some people think CQRS and Vertical Slice Architecture are too complicated and take forever to set up. This template proves that's not true. Here's what it brings:

- Zero Setup: You can start coding right away-no painful setup required.
- Simple Structure: The project layout is clean and easy to scale.
- Consistent Patterns: Keeps your codebase organized and predictable.
- Easy to Test: Write modular, testable code without extra hassle.
 
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
