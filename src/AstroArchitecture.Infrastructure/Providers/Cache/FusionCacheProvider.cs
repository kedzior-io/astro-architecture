using AstroArchitecture.Core.Constants;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace AstroArchitecture.Infrastructure.Providers.Cache;

public class FusionCacheProvider : ICacheProvider
{
    private readonly IFusionCache _cache;
    private readonly TimeSpan _defaultTimeSpan = TimeSpan.FromDays(1);

    public FusionCacheProvider(IConfiguration configuration)
    {
        if (_cache is null)
        {
            var redis = new RedisCache(new RedisCacheOptions()
            {
                Configuration = configuration.GetConnectionString(ConnectionStrings.Redis)
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
                Configuration = configuration.GetConnectionString(ConnectionStrings.Redis)
            });

            _cache.SetupBackplane(backplane);
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        return await _cache.GetOrDefaultAsync<T>(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan cacheDuration)
    {
        await _cache.SetAsync(key, value, cacheDuration);
    }
}