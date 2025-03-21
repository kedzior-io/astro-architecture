using AstroArchitecture.Handlers.Handlers.Abstractions;

namespace AstroArchitecture.Handlers.Handlers.Customers.Commands;

public static class CreateCustomer
{
    public sealed record Command(string FirstName, string LastName, string Email) : ICommand<IHandlerResponse<Response>>;
    public sealed record Response(Guid CustomerId);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var customer = new Customer(command.FirstName, command.LastName, command.Email);
            await DbContext.Customers.AddAsync(customer, ct);
            await DbContext.SaveChangesAsync(ct);

            return Success(new Response(customer.Id));
        }
    }
}