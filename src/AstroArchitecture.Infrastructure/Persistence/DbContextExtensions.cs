using AstroArchitecture.Domain.Abstractions;
using MinimalCqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AstroArchitecture.Infrastructure.Persistence;

public static class DbContextExtensions
{
    private static readonly Dictionary<Type, Func<IDomainEvent, object>> _factoryCache = new();

    private static object WrapDomainEvent(IDomainEvent domainEvent)
    {
        var type = domainEvent.GetType();

        if (!_factoryCache.TryGetValue(type, out var factory))
        {
            var wrapperType = typeof(DomainEvent<>).MakeGenericType(type);
            var ctor = wrapperType.GetConstructor(new[] { type })!;

            var param = Expression.Parameter(typeof(IDomainEvent), "e");
            var cast = Expression.Convert(param, type);
            var newExpr = Expression.New(ctor, cast);
            var lambda = Expression.Lambda<Func<IDomainEvent, object>>(newExpr, param);

            factory = lambda.Compile();
            _factoryCache[type] = factory;
        }

        return factory(domainEvent);
    }

    private static bool InheritsFromGenericType(Type type, Type genericType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return true;

            type = type.BaseType!;
        }

        return false;
    }

    public static async Task DispatchDomainEventsAsync(this IDbContext context, CancellationToken cancellationToken = default)
    {
        var entities = context.Changes.Entries()
            .Where(e => InheritsFromGenericType(e.Entity.GetType(), typeof(Entity<>)))
            .Select(e => (dynamic)e.Entity)
            .Where(e => e.DomainEvents?.Count > 0)
            .ToList();

        foreach (var entity in entities)
        {
            foreach (IDomainEvent domainEvent in entity.DomainEvents)
            {
                var wrapped = WrapDomainEvent(domainEvent);
                await EventBus.PublishAsync((dynamic)wrapped, cancellationToken);
            }

            entity.ClearDomainEvent();
        }
    }
}