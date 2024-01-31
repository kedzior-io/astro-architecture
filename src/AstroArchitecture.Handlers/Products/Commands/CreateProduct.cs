namespace AstroArchitecture.Handlers;

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