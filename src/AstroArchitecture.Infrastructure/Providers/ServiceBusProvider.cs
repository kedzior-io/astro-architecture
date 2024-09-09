using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace AstroArchitecture.Infrastructure.Providers;

public class ServiceBusProvider(ServiceBusClient serviceBusClient) : IServiceBusProvider
{
    public async Task Send(object message, string queueName, CancellationToken ct)
    {
        var sender = serviceBusClient.CreateSender(queueName);

        var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));

        await sender.SendMessageAsync(serviceBusMessage, ct);
    }

    public async Task Enqueue(object message, string queueName, int delayInMinutes, CancellationToken ct)
    {
        var sender = serviceBusClient.CreateSender(queueName);

        var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));

        serviceBusMessage.ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddMinutes(delayInMinutes);

        await sender.SendMessageAsync(serviceBusMessage, ct);
    }
}