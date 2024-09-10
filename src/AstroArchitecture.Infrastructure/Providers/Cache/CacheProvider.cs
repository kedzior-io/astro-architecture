using AstroArchitecture.Core.Constants;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace AstroArchitecture.Infrastructure.Providers.Cache;

public class CacheProvider : ICacheContext
{
    private readonly IFusionCache _cache;
    private readonly TimeSpan _defaultTimeSpan = TimeSpan.FromDays(365);

    public CacheProvider()
    {
        if (_cache is null)
        {
            var redis = new RedisCache(new RedisCacheOptions()
            {
                Configuration = ConnectionStrings.Redis
            });

            var serializer = new FusionCacheSystemTextJsonSerializer(new System.Text.Json.JsonSerializerOptions { IncludeFields = true });

            _cache = new FusionCache(new FusionCacheOptions()
            {
                DefaultEntryOptions = new FusionCacheEntryOptions
                {
                    Duration = _defaultTimeSpan,
                }
            });

            _cache.SetupDistributedCache(redis, serializer);

            var backplane = new RedisBackplane(new RedisBackplaneOptions()
            {
                Configuration = ConnectionStrings.Redis
            });

            _cache.SetupBackplane(backplane);
        }
    }

    public async Task<T> GetOrSet<T>(string key, Func<CancellationToken, Task<T>> factory, CancellationToken ct)
    {
        return await _cache.GetOrSetAsync(key, factory, _defaultTimeSpan, ct);
    }

    public async Task Set<T>(string key, T value, CancellationToken ct)
    {
        await _cache.SetAsync(key, value, _defaultTimeSpan, ct);
    }
}