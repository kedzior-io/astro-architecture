using AstroArchitecture.Infrastructure.Persistence;
using AstroArchitecture.Infrastructure.Providers.Cache;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AstroArchitecture.Handlers;

public sealed class AzureFunctionsHandlerContext(IServiceScopeFactory serviceScopeFactory, ICacheContext cacheContext, ILogger logger) : IHandlerContext
{
    public IDbContext DbContext => GetDbContextUsingServiceScopeFactory();
    public ICacheContext CacheContext { get; private set; } = cacheContext;
    public ILogger Logger { get; private set; } = logger;

    private IDbContext GetDbContextUsingServiceScopeFactory()
    {
        var scope = serviceScopeFactory.CreateScope() ?? throw new ArgumentNullException("Unable to create scope");

        return scope.ServiceProvider.GetRequiredService<IDbContext>();
    }
}