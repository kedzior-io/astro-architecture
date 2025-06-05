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
        var response = await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await DbContext.DispatchDomainEventsAsync(cancellationToken).ConfigureAwait(false);

        return response;
    }

    //private static async Task DispatchDomainEventsAsync(IDbContext dbContext, CancellationToken cancellationToken = default)
    //{
    //    var entities = dbContext.Changes.Entries()
    //        .Where(e => InheritsFromGenericType(e.Entity.GetType(), typeof(Entity<>)))
    //        .Select(e => e.Entity)
    //        .Cast<dynamic>()
    //        .Where(e => e.DomainEvents != null && e.DomainEvents.Count > 0)
    //        .ToList();

    //    foreach (dynamic entitity in entities)
    //    {
    //        foreach (IDomainEvent domainEvent in entitity.DomainEvents)
    //        {
    //            await EventBus.PublishAsync(new DomainEvent(domainEvent), cancellationToken);
    //        }

    //        entitity.ClearDomainEvent();
    //    }
    //}

    //private static bool InheritsFromGenericType(Type type, Type genericType)
    //{
    //    while (type != null && type != typeof(object))
    //    {
    //        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
    //            return true;

    //        type = type.BaseType!;
    //    }
    //    return false;
    //}
}

public abstract class CommandHandler<TCommand>(IHandlerContext context) : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}