using AstroArchitecture.Domain.Customers.Events;
using Microsoft.Extensions.Caching.Memory;

namespace AstroArchitecture.Handlers.EventHandlers.Customers;

// Proposal
// TODO: Ideal read store consumption
// ReadDbContext: Redis with data persistence

// TODO: create manual readdb store sync
public sealed class AddressCreatedEventHandler(IMemoryCache _memoryCache /* IReadDbContext _readDbContext */) : INotificationHandler<AddressCreatedEvent>
{    
    public async Task Handle(AddressCreatedEvent notification, CancellationToken cancellationToken)
    {

        SetAddressList(notification);
        SetAddress(notification);
        SetAddressFullList(notification);
    }

    private void SetAddressList(AddressCreatedEvent notification)
    {
        var key = CacheKeys.CustomerAddressList(notification.CustomerId);

        var address = new AddressListReadModel(notification);

        var customerAddressList = _memoryCache.Get<List<AddressListReadModel>>(key) ?? [];

        customerAddressList.Add(address);

        _memoryCache.Set(key, customerAddressList, DateTimeOffset.MaxValue);
    }

    private void SetAddress(AddressCreatedEvent notification)
    {
        var key = CacheKeys.CustomerAddress(notification.CustomerId, notification.AddressId);

        var address = new AddressReadModel(notification);

        _memoryCache.Set(key, address, DateTimeOffset.MaxValue);
    }

    private void SetAddressFullList(AddressCreatedEvent notification)
    {
        var address = new AddressFullListReadModel(notification);

        var key = CacheKeys.AddressFullList;

        var allAddressList = _memoryCache.Get<List<AddressFullListReadModel>>(key) ?? [];

        allAddressList.Add(address);

        _memoryCache.Set(key, allAddressList, DateTimeOffset.MaxValue);
    }

    private static class CacheKeys
    {
        public static string CustomerAddressList(Guid customerId) => $"{customerId}-{nameof(CustomerAddressList)}";
        public static string CustomerAddress(Guid customerId, Guid addressId) => $"{customerId}-{addressId}-{nameof(CustomerAddress)}";

        public static string AddressFullList =>nameof(AddressFullList);
    } 
}

// ----------------------------------------------------------------------

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

public class AddressFullListReadModel(AddressCreatedEvent addressCreatedEvent) : AddressReadModel(addressCreatedEvent)
{
}





// TODO: add to AstroCQRS
public interface INotificationHandler<T> { }

// TODO: add to AstroCQRS
public interface IPublisher {
    Task Publish(object notifications, CancellationToken cancellationToken);
}