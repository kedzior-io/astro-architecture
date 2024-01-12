using AstroArchitecture.Core.Constants;
using AstroArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AstroArchitecture.Infrastructure.Persistence;

public interface IDbContext : IDisposable
{
    public DbSet<Order> Orders { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null);
}

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IDbContext
{
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(ConnectionStrings.Sqlite);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, [CallerMemberName] string? callerFunction = null, [CallerFilePath] string? callerFile = null) =>
        await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
}