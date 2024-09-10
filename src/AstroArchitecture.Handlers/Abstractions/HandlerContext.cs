using AstroArchitecture.Infrastructure.Persistence;
using AstroArchitecture.Infrastructure.Providers.Cache;
using Serilog;

namespace AstroArchitecture.Handlers;

public sealed class HandlerContext(IDbContext dbContext, ICacheContext cache, ILogger logger) : IHandlerContext
{
    public IDbContext DbContext { get; private set; } = dbContext;
    public ICacheContext CacheContext { get; private set; } = cache;
    public ILogger Logger { get; private set; } = logger;
}