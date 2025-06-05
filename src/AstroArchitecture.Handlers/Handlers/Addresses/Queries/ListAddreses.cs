﻿using AstroArchitecture.Handlers.Abstractions;
using AstroArchitecture.Handlers.Handlers.Addresses.Queries.Models;
using Microsoft.AspNetCore.Components.Web;

namespace AstroArchitecture.Handlers.Handlers.Addresses.Queries;

public static class ListAddresses
{
    public sealed record Query(Guid Id) : IQuery<IHandlerResponse<Response>>;

    public sealed class Response(IReadOnlyCollection<Address> addresses)
    {
        public IReadOnlyCollection<AddressListModel> Addresses { get; private set; } = addresses.Select(AddressListModel.Create).ToList();
    }

    public sealed class Handler(IHandlerContext context) : QueryHandler<Query, Response>(context)
    {
        public override async Task<IHandlerResponse<Response>> ExecuteAsync(Query query, CancellationToken ct)
        {
            var customer = await DbContext.Customers
                .AsNoTracking()
                .Include(x => x.Addresses)
                .Where(x => x.Id == query.Id)
                .SingleOrDefaultAsync(ct);

            if (customer is null)
            {
                return Error($"Customer {query.Id} not found.");
            }

            return Success(new Response(customer.Addresses.ToList()));

            // Proposal
            // TODO: Ideal read store consumption

            //var addressList = await ReadDbContext
            //    .Customers
            //    .CustomerList
            //    .WithCustomerId(query.Id);

            // return Success(new Response(addressList));
        }
    }
}