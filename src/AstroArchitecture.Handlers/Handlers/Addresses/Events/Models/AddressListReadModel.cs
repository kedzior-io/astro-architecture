using AstroArchitecture.Domain.Customers.Events;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Events.Models;

public class AddressListReadModel
{
    public Guid AddressId { get; set; }
    public string Name { get; set; }

    public AddressListReadModel(AddressCreatedEvent addressCreatedEvent)
    {
        AddressId = addressCreatedEvent.AddressId;
        Name = addressCreatedEvent.Name;
    }
}