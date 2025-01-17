﻿using AstroArchitecture.Handlers.Customers;

namespace AstroArchitecture.Handlers;

public static class ListProducts
{
    public sealed record Query() : IQuery<IHandlerResponse<Response>>;

    public sealed class Response(IReadOnlyCollection<Product> products)
    {
        public IReadOnlyCollection<ProductListModel> Products { get; private set; } = products.Select(ProductListModel.Create).ToList();
    }

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var products = await DbContext.Products
                .AsNoTracking()
                .ToListAsync(ct);

            return Success(new Response(products));
        }
    }
}