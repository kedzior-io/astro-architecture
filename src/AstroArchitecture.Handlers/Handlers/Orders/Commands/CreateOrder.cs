using AstroArchitecture.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Orders.Models;

namespace AstroArchitecture.Handlers.Handlers.Orders.Commands;

public static class CreateOrder
{
    public sealed record Command(Guid CustomerId, Guid AddressId, IReadOnlyCollection<ProductQuantity> ProductQuantites) : ICommand<IHandlerResponse<Response>>;

    public record ProductQuantity(int ProductId, int Quantity);

    public sealed record Response(OrderListModel Order);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.AddressId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ProductQuantites)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var productIds = command.ProductQuantites.Select(x => x.ProductId);

            var products = await DbContext.Products
                .Where(x => productIds.Contains(x.Id))
                .ToListAsync(ct);

            var productQuantites = new Dictionary<Product, int>();

            var validationErrors = new List<string>();

            foreach (var product in products)
            {
                var quantity = command.ProductQuantites.Single(x => x.ProductId == product.Id).Quantity;

                if (product.Stock < quantity)
                {
                    validationErrors.Add($"Product {product.Name} is out of stock.");
                    continue;
                }

                productQuantites.Add(product, quantity);

                product.SubtractStock(quantity);
            }

            if (validationErrors.Count > 0)
            {
                // This is just for example purposes, normally we would return a proper validation response object
                return Error(string.Join(',', validationErrors));
            }

            var customer = await DbContext.Customers
                .Include(x => x.Addresses)
                .Where(x => x.Id == command.CustomerId)
                .SingleOrDefaultAsync(ct);

            if (customer is null)
            {
                return Error($"Customer {command.CustomerId} not found.");
            }

            var address = customer.Addresses
                .Where(x => x.Id == command.AddressId)
                .SingleOrDefault();

            if (address is null)
            {
                return Error($"Address {command.AddressId} not found.");
            }

            var order = new Order(productQuantites, customer, address);

            await DbContext.Orders.AddAsync(order, ct);
            await DbContext.SaveChangesAsync();

            var response = new Response(OrderListModel.Create(order));

            return Success(response);
        }
    }
}