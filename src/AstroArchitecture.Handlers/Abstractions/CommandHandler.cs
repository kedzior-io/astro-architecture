using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers;

public abstract class CommandHandler<TCommand, TResponse>(IHandlerContext context) : Handler<TCommand, TResponse> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}

public abstract class CommandHandler<TCommand>(IHandlerContext context) : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{

    protected readonly ILogger Logger = context.Logger;
    protected readonly IDbContext DbContext = context.DbContext;
}
