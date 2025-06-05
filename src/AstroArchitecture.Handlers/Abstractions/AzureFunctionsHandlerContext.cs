using AstroArchitecture.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AstroArchitecture.Handlers.Abstractions;

public sealed class AzureFunctionsHandlerContext(IServiceScopeFactory serviceScopeFactory, ILogger logger) : IHandlerContext
{
    public IDbContext DbContext => GetDbContextUsingServiceScopeFactory();
    public ILogger Logger { get; private set; } = logger;

    private IDbContext GetDbContextUsingServiceScopeFactory()
    {
        var scope = serviceScopeFactory.CreateScope() ?? throw new ArgumentNullException("Unable to create scope");

        return scope.ServiceProvider.GetRequiredService<IDbContext>();
    }
}