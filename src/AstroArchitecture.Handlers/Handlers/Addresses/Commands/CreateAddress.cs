using AstroArchitecture.Domain.Customers.Events;
using AstroArchitecture.Handlers.EventHandlers.Customers;
using AstroArchitecture.Handlers.Handlers.Abstractions;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Commands;

public static class CreateAddress
{
    public sealed record Command(Guid CustomerId, string Name, string Street, string City, string Country, string ZipCode) : ICommand<IHandlerResponse<Response>>;
    public sealed record Response(Guid AddressId);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Street)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.City)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ZipCode)
                 .NotNull()
                 .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var customer = await DbContext.Customers
                .Include(x => x.Addresses)
                .Where(x => x.Id == command.CustomerId)
                .SingleOrDefaultAsync(ct);

            if (customer is null)
            {
                return Error($"Customer {command.CustomerId} not found.");
            }

            var address = customer.AddAddress(command.Name, command.Street, command.City, command.Country, command.ZipCode);

            await SaveChangesAsync(ct);

            return Success(new Response(address.Id));
        }
    }
}