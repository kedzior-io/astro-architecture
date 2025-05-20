using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Domain.Customers.Events;

public class AddressCreatedEvent : IDomainEvent
{
    public Guid CustomerId { get; set; }
    public Guid AddressId { get; set; }
    public string Name { get; set; }
    public string ZipCode { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public AddressCreatedEvent(Guid customerId, Address address)
    {
        AddressId = address.Id;
        CustomerId = customerId;
        Name = address.Name;
        ZipCode = address.ZipCode;
        CreatedAtUtc = DateTime.UtcNow;
    }
}