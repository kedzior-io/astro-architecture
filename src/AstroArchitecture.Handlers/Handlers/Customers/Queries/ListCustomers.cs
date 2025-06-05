using AstroArchitecture.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Customers.Models;

namespace AstroArchitecture.Handlers.Handlers.Customers.Queries;

public static class ListCustomers
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;

    public sealed class Response(IReadOnlyCollection<Customer> customers)
    {
        public IReadOnlyCollection<CustomerListModel> Customers { get; private set; } = customers.Select(CustomerListModel.Create).ToList();
    }

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var orders = await DbContext.Customers
                .AsNoTracking()
                .ToListAsync(ct);

            return Success(new Response(orders));
        }
    }
}