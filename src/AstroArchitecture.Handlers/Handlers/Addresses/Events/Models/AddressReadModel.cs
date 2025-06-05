using AstroArchitecture.Domain.Customers.Events;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Events.Models;

public class AddressReadModel
{
    public Guid AddressId { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
    public string ZipCode { get; set; }

    public AddressReadModel(AddressCreatedEvent addressCreatedEvent)
    {
        AddressId = addressCreatedEvent.AddressId;
        CustomerId = addressCreatedEvent.CustomerId;
        Name = addressCreatedEvent.Name;
        ZipCode = addressCreatedEvent.ZipCode;
    }
}