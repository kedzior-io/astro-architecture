using Ardalis.GuardClauses;
using AstroArchitecture.Core.Enums;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class Order : Entity, IAggregateRoot
{
    public string CustomerName { get; private set; } = null!;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public OrderStatus OrderStatus { get; private set; } = OrderStatus.Draft;
    public string UserId { get; private set; } = null!;

    private Order()
    {
        // EF
    }

    public Order(string name, string userId)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(userId);

        CustomerName = name;
        UserId = userId;
    }

    public static Order CreateDraft(string name)
    {
        return new Order(name, "1");
    }

    public static Order Create(string name)
    {
        return new Order(name, "1");
    }

    public static Order Create(string name, string userId)
    {
        return new Order(name, userId);
    }
}