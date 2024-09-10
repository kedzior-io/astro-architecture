namespace AstroArchitecture.Infrastructure.Providers.ServiceBus;

public interface IServiceBusProvider
{
    Task Send(object message, string queueName, CancellationToken ct);

    Task Enqueue(object message, string queueName, int delayInMinutes, CancellationToken ct);
}