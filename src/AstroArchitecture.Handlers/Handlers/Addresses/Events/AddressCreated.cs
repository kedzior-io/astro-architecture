using AstroArchitecture.Domain.Customers.Events;
using AstroArchitecture.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Addresses.Events.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Events;

public static class AddressCreated
{
    public sealed class Handler(IMemoryCache _memoryCache, IHandlerContext context) : Abstractions.EventHandler<DomainEvent<AddressCreatedEvent>>(context)
    {
        public override async Task ExecuteAsync(DomainEvent<AddressCreatedEvent> @event, CancellationToken ct)
        {
            SetAddressList(@event.Payload);
            SetAddress(@event.Payload);
            await Task.CompletedTask;
        }

        private void SetAddressList(AddressCreatedEvent eventNotification)
        {
            var key = CacheKeys.CustomerAddressList(eventNotification.CustomerId);

            var address = new AddressListReadModel(eventNotification);

            var customerAddressList = _memoryCache.Get<List<AddressListReadModel>>(key) ?? [];

            customerAddressList.Add(address);

            _memoryCache.Set(key, customerAddressList, DateTimeOffset.MaxValue);
        }

        private void SetAddress(AddressCreatedEvent eventNotification)
        {
            var key = CacheKeys.CustomerAddress(eventNotification.CustomerId, eventNotification.AddressId);

            var address = new AddressReadModel(eventNotification);

            _memoryCache.Set(key, address, DateTimeOffset.MaxValue);
        }

        private static class CacheKeys
        {
            public static string CustomerAddressList(Guid customerId) => $"{customerId}-{nameof(CustomerAddressList)}";

            public static string CustomerAddress(Guid customerId, Guid addressId) => $"{customerId}-{addressId}-{nameof(CustomerAddress)}";

            public static string AddressFullList => nameof(AddressFullList);
        }
    }
}