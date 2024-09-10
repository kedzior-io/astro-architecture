namespace AstroArchitecture.Infrastructure.Providers.Cache;

public interface ICacheContext
{
    Task Set<T>(string key, T value, CancellationToken ct);

    Task<T> GetOrSet<T>(string key, Func<CancellationToken, Task<T>> factory, CancellationToken ct);
}