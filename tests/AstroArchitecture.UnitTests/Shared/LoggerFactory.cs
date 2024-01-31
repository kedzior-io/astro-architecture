using Serilog;

namespace AstroArchitecture.UnitTests.Shared;

public static class LoggerFactory
{
    public static ILogger Create()
    {
        var logger = new LoggerConfiguration()
            .CreateLogger();

        Log.Logger ??= logger;

        return logger;
    }
}