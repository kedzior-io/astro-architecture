using AstroArchitecture.Core.Cache;
using AstroArchitecture.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Addresses.Events.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Queries;

public static class GetCachedAddreses
{
    public sealed record Query(Guid Id) : IQuery<IHandlerResponse<Response>>;

    public sealed record Response(AddressReadModel Addresses);

    public sealed class Handler(IMemoryCache memoryCache, IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var cacheKey = CacheKeys.CustomerAddress(query.Id);

            var customerAddress = memoryCache.Get<AddressReadModel>(cacheKey);

            if (customerAddress is null)
            {
                return Error($"Address not found {query.Id} not found.");
            }

            return Success(new Response(customerAddress));
        }
    }
}