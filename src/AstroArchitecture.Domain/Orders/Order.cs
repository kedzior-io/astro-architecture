using Ardalis.GuardClauses;
using AstroArchitecture.Core.Enums;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain;

public class Order : Entity<Guid>, IAggregateRoot
{
    public Guid CustomerId { get; private set; }
    public OrderAddress Address { get; private set; }
    public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
    public OrderStatus OrderStatus { get; private set; } = OrderStatus.Draft;
    public virtual ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
    public virtual Customer Customer { get; private set; }

    private Order()
    {
        // EF
    }

    public Order(IDictionary<Product, int> productQuantities, Customer customer, Address address)
    {
        Guard.Against.NullOrEmpty(productQuantities);

        Id = Guid.NewGuid();

        foreach (var entry in productQuantities)
        {
            if (entry.Value <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0", nameof(productQuantities));
            }

            Items.Add(new OrderItem(entry.Key, entry.Value));
        }

        Customer = customer;
        Address = OrderAddress.Create(address, customer);

        CustomerId = customer.Id;
    }
}