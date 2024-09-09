using AstroArchitecture.Handlers.Customers.Models;

namespace AstroArchitecture.Handlers;

public static class CustomerUpdated
{
    public sealed record Command(Guid CustomerId) : ICommand<IHandlerResponse>;

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command>(context)
    {
        public override async Task<IHandlerResponse> ExecuteAsync(Command command, CancellationToken ct)
        {
            var customer = await DbContext.Customers
                .Where(x => x.Id == command.CustomerId)
                .SingleOrDefaultAsync(ct);

            if (customer is null)
            {
                return Error("Customer not found");
            }

            return Success();
        }
    }
}