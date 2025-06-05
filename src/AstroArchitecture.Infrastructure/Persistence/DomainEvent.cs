using MinimalCqrs;

namespace AstroArchitecture.Infrastructure.Persistence;

public sealed class DomainEvent<IDomainEvent> : IEvent
{
    public IDomainEvent Payload { get; }

    public DomainEvent(IDomainEvent domainEvent)
    {
        Payload = domainEvent;
    }
}