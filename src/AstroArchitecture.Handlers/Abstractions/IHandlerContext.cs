using AstroArchitecture.Infrastructure.Persistence;
using Serilog;

namespace AstroArchitecture.Handlers;

public interface IHandlerContext
{
    IDbContext DbContext { get; }
    ILogger Logger { get; }
}