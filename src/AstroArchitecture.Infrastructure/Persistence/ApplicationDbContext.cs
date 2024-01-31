using AstroArchitecture.Core.Constants;
using AstroArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AstroArchitecture.Infrastructure.Persistence;

public interface IDbContext : IDisposable
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null);
}

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        if (!builder.IsConfigured)
        {
            builder.UseSqlite(ConnectionStrings.Sqlite);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // The entity type 'OrderItem' has composite key '{'OrderId', 'Id'}' which is configured to use generated values. SQLite does not support generated values on composite keys.

        modelBuilder.Entity<Order>().OwnsOne(x => x.Address);
        modelBuilder.Entity<Order>().OwnsMany(x => x.Items);

        //modelBuilder.Entity<OrderItem>().HasOne(x => x.Order);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null) =>
        await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}