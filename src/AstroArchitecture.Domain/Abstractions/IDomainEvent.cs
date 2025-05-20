namespace AstroArchitecture.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime CreatedAtUtc { get; protected set; }
}