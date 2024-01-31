using AstroArchitecture.Core.Enums;

namespace AstroArchitecture.Domain.Abstractions
{
    public abstract class Entity<T>
    {
        public T Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; protected set; }
        public EntityStatus EntityStatus { get; protected set; } = EntityStatus.Active;
    }
}