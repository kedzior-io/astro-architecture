using AstroArchitecture.Domain.Customers.Events;
using AstroArchitecture.Domain;
using AstroArchitecture.Infrastructure.Persistence;
using Serilog;
using System.Runtime.CompilerServices;
using AstroArchitecture.Handlers.EventHandlers.Customers;
using Microsoft.EntityFrameworkCore;
using AstroArchitecture.Domain.Abstractions;
using System.Reflection;

namespace AstroArchitecture.Handlers.Handlers.Abstractions;

public abstract class CommandHandler<TCommand, TResponse>(IHandlerContext context) : Handler<TCommand, TResponse> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
    private readonly IPublisher _eventPublisher = context.EventPublisher;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null)
    {
        var response = await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var entities = DbContext.Changes.Entries()
           .Where(e => e.Entity is Entity<object> &&
                       (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
           .Select(e => e.Entity)
           .Cast<Entity<object>>()
           .ToList();

        foreach (var entitity in entities)
        {
            foreach (var domainEvent in entitity.DomainEvents)
            {
                await _eventPublisher.Publish(domainEvent, cancellationToken);
            }

            entitity.ClearDomainEvent();
        }

        return response;
    }
}

public abstract class CommandHandler<TCommand>(IHandlerContext context) : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}