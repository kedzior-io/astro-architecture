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
}

public abstract class CommandHandler<TCommand>(IHandlerContext context) : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}