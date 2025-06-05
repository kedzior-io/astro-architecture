using AstroArchitecture.Handlers.Abstractions;

namespace AstroArchitecture.UnitTests.Shared;

public static class HandlerContextFactory
{
    public static IHandlerContext GetHandlerContext(IDbContext dbContext, string? userId = null, string? email = null)
    {
        return new HandlerContext(dbContext, LoggerFactory.Create());
    }
}