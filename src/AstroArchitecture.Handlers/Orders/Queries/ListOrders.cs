namespace AstroArchitecture.Handlers;

public static class ListOrders
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;

    public sealed class Response(IReadOnlyCollection<Order> orders)
    {
        public IReadOnlyCollection<OrderListModel> Orders { get; private set; } = orders.Select(OrderListModel.Create).ToList();
    }

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var orders = await DbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ToListAsync(ct);

            return Success(new Response(orders));
        }
    }
}