using AstroArchitecture.Api.Middlewares.Cache;
using AstroArchitecture.Infrastructure.Providers.Cache;

namespace AstroArchitecture.Api.Cache;

public class CacheMiddleware(RequestDelegate next, ICacheProvider casheProvider)
{
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var cacheMetadata = endpoint?.Metadata.GetMetadata<CacheMetadata>();

        if (cacheMetadata is not { IsCacheEnabled: true })
        {
            await next(context);
            return;
        }

        var cacheKey = GenerateCacheKey(context.Request);

        var cachedResponse = await casheProvider.GetAsync<CachedResponse>(cacheKey);

        if (cachedResponse is not null)
        {
            context.Response.ContentType = cachedResponse.ContentType;
            context.Response.StatusCode = cachedResponse.StatusCode;
            await context.Response.Body.WriteAsync(cachedResponse.Body, 0, cachedResponse.Body.Length);

            return;
        }

        var originalBodyStream = context.Response.Body;
        await using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status200OK)
        {
            var responseBody = responseBodyStream.ToArray();
            var responseCache = new CachedResponse
            {
                ContentType = context.Response.ContentType,
                StatusCode = context.Response.StatusCode,
                Body = responseBody
            };

            await casheProvider.SetAsync(cacheKey, responseCache, _cacheDuration);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
    }

    /// <summary>
    /// Generate a cashKey based on request Url and connection strings
    /// </summary>
    /// <returns></returns>
    private static string GenerateCacheKey(HttpRequest request)
    {
        var path = request.Path.ToString();
        var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
        return $"{path}{queryString}";
    }

    private class CachedResponse
    {
        public string? ContentType { get; set; }
        public int StatusCode { get; set; }
        public byte[] Body { get; set; } = [];
    }
}