using AstroArchitecture.Domain.Customers.Events;
using AstroArchitecture.Domain;
using AstroArchitecture.Infrastructure.Persistence;
using Serilog;
using System.Runtime.CompilerServices;
using AstroArchitecture.Handlers.EventHandlers.Customers;
using Microsoft.EntityFrameworkCore;

namespace AstroArchitecture.Handlers.Handlers.Abstractions;

public abstract class CommandHandler<TCommand, TResponse>(IHandlerContext context) : Handler<TCommand, TResponse> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
    private readonly IPublisher _eventPublisher = context.EventPublisher;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null)
    {
        var response = await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        //var modifiedEntities = context.DbContext.Changes
        //    .Entries()
        //    .Where(e => e.State == EntityState.Modified)
        //    .Select(e => e.Entity)
        //    .ToList();

        //foreach (var entity as Entity in modifiedEntities)
        //{
        //    foreach (var entity in entity)
        //    {
        //        await _eventPublisher.Publish(entitycancellationToken);
        //    }
        //}

        return response;
    }
}

public abstract class CommandHandler<TCommand>(IHandlerContext context) : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}