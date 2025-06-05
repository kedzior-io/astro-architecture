using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers.Abstractions;

public abstract class EventHandler<TEvent>(IHandlerContext context) : MinimalCqrs.EventHandler<TEvent> where TEvent : IHandlerMessage
{
    protected readonly ILogger Logger = context.Logger;

    protected readonly IDbContext DbContext = context.DbContext;
}