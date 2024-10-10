namespace AstroArchitecture.Api.Middlewares.Cache;

public static class CacheMiddlewareExtensions
{
    public static IEndpointConventionBuilder UseCache(this IEndpointConventionBuilder builder)
    {
        builder.Add(endpointBuilder =>
        {
            endpointBuilder.Metadata.Add(new CacheMetadata { IsCacheEnabled = true });
        });

        return builder;
    }
}