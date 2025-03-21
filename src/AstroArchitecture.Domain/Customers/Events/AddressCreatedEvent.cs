namespace AstroArchitecture.Domain.Customers.Events;
public class AddressCreatedEvent
{
    public Guid AddressId { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
    public string ZipCode { get; set; }

    public AddressCreatedEvent(Guid customerId, Address address)
    {
        AddressId = address.Id;
        CustomerId = customerId;
        Name = address.Name;
        ZipCode = address.ZipCode;
    }
}