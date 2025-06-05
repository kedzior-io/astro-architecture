using AstroArchitecture.Infrastructure.Persistence;
using Serilog;
using System.Runtime.CompilerServices;
using AstroArchitecture.Domain.Abstractions;

namespace AstroArchitecture.Handlers.Abstractions;

public abstract class CommandHandler<TCommand, TResponse>(IHandlerContext context) : Handler<TCommand, TResponse> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null)
    {
        var entities = DbContext.Changes.Entries()
            .Where(e =>
                (e.State == EntityState.Added ||
                 e.State == EntityState.Modified ||
                 e.State == EntityState.Deleted) &&
                InheritsFromGenericType(e.Entity.GetType(), typeof(Entity<>)))
            .Select(e => e.Entity)
            .ToList();

        var response = await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        foreach (dynamic entitity in entities)
        {
            // TODO: entitity.DomainEvents is empty

            foreach (var domainEvent in entitity.DomainEvents)
            {
                await EventBus.PublishAsync(domainEvent, cancellationToken);
            }

            entitity.ClearDomainEvent();
        }

        return response;
    }

    private static bool InheritsFromGenericType(Type type, Type genericType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;

            type = type.BaseType!;
        }
        return false;
    }
}

public abstract class CommandHandler<TCommand>(IHandlerContext context) : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}