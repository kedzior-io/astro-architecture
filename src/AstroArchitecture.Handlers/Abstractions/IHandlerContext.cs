using AstroArchitecture.Infrastructure.Persistence;
using AstroArchitecture.Infrastructure.Providers.Cache;
using Serilog;

namespace AstroArchitecture.Handlers;

public interface IHandlerContext
{
    IDbContext DbContext { get; }
    ICacheContext CacheContext { get; }
    ILogger Logger { get; }
}