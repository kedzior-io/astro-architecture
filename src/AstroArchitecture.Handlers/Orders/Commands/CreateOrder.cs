using FluentValidation;

namespace AstroArchitecture.Handlers;

/*
 * An example of a Command with parameters and response
 */

public static class CreateOrder
{
    public sealed record Command(string CustomerName, decimal Total) : ICommand<IHandlerResponse<Response>>;
    public sealed record Response(OrderListModel Order);

    public sealed class CreateOrderValidator : Validator<Command>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotNull()
                .NotEmpty();
        }
    }

    public sealed class Handler(IHandlerContext context) : CommandHandler<Command, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command command, CancellationToken ct)
        {
            var order = Order.Create(command.CustomerName);

            await DbContext.Orders.AddAsync(order, ct);
            await DbContext.SaveChangesAsync(ct);

            var response = new Response(OrderListModel.Create(order));

            return Success(response);
        }
    }
}