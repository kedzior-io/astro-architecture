using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers;

public abstract class CommandHandler<TCommand, TResponse> : Handler<TCommand, TResponse> where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger;
    protected readonly IDbContext DbContext;

    protected CommandHandler(IHandlerContext context)
    {
        Logger = context.Logger;
        DbContext = context.DbContext;
    }
}