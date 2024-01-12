namespace AstroArchitecture.Handlers;

/*
 * An example of a Query without parameters
 */

public static class ListOrders
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;

    public sealed class Response(IReadOnlyCollection<Domain.Order> orders)
    {
        public IReadOnlyCollection<OrderListModel> Orders { get; private set; } = orders.Select(OrderListModel.Create).ToList();
    }

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var orders = await DbContext
                .Orders
                .ToListAsync(ct);

            return Success(new Response(orders));
        }
    }
}