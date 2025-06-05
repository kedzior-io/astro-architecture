using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Handlers.Abstractions;

public sealed class DomainEvent<TEvent> : IHandlerMessage where TEvent : IDomainEvent
{
    public TEvent Payload { get; }

    public DomainEvent(TEvent domainEvent)
    {
        Payload = domainEvent;
    }
}