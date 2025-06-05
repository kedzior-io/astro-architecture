using AstroArchitecture.Domain.Customers.Events;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Events.Models;

public class AddressFullListReadModel(AddressCreatedEvent addressCreatedEvent) : AddressReadModel(addressCreatedEvent)
{
}