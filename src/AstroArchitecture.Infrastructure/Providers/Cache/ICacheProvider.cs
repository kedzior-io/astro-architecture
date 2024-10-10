namespace AstroArchitecture.Infrastructure.Providers.Cache;

public interface ICacheProvider
{
    Task SetAsync<T>(string key, T value, TimeSpan cacheDuration);

    Task<T?> GetAsync<T>(string key);
}