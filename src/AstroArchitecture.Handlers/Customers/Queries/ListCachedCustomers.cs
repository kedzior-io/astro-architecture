using AstroArchitecture.Core.Constants;

namespace AstroArchitecture.Handlers;

public static class ListCachedCustomers
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;

    public sealed record Response(IReadOnlyCollection<CustomerListModel> Customers);

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var customers = await CacheContext.GetOrSet(
                CacheContextKeys.CustomersList,
                _ => GetCustomers(ct),
                ct);

            return Success(new Response(customers));
        }

        private async Task<List<CustomerListModel>> GetCustomers(CancellationToken ct)
        {
            var customers = await DbContext.Customers
                .AsNoTracking()
                .ToListAsync(ct);

            return customers.Select(CustomerListModel.Create).ToList();
        }
    }
}