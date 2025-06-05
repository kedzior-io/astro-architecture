using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers.Abstractions;

public interface IHandlerContext
{
    IDbContext DbContext { get; }
    ILogger Logger { get; }
}