using AstroArchitecture.Handlers.EventHandlers.Customers;
using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers.Handlers.Abstractions;

public interface IHandlerContext
{
    IDbContext DbContext { get; }
    IPublisher EventPublisher { get; }
    ILogger Logger { get; }
}