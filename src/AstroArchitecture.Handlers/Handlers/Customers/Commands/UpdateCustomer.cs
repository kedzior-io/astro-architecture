using AstroArchitecture.Handlers.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Customers.Models;
using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace AstroArchitecture.Handlers.Handlers.Customers.Commands;

public static class UpdateCustomer
{
    public sealed record Command(Guid CustomerId) : ICommand<IHandlerResponse>;

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull()
                .NotEmpty();
        }

        public sealed class Handler(IHandlerContext context, ServiceBusClient serviceBusClient) : CommandHandler<Command>(context)
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

                await SendServiceBusMessage(customer);

                return Success();
            }

            private async Task SendServiceBusMessage(Customer customer)
            {
                var sender = serviceBusClient.CreateSender("customer-updated");

                var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(new CustomerModel(customer.Id)));

                await sender.SendMessageAsync(serviceBusMessage);
            }
        }
    }
}