using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers;

public sealed class HandlerContext : IHandlerContext
{
    public IDbContext DbContext { get; private set; }
    public ILogger Logger { get; private set; }

    public HandlerContext(IDbContext dbContext, ILogger logger)
    {
        DbContext = dbContext;
        Logger = logger;
    }
}