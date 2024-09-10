﻿using AstroArchitecture.Infrastructure.Providers.Cache;

namespace AstroArchitecture.UnitTests.Shared;

public static class HandlerContextFactory
{
    public static IHandlerContext GetHandlerContext(ApplicationDbContext dbContext, string? userId = null, string? email = null)
    {
        return new HandlerContext(dbContext, new CacheProvider(), LoggerFactory.Create());
    }
}