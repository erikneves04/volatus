using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volatus.Data.Context;
using Volatus.Domain.Entities;
using Volatus.Domain.Interfaces.Entities;

namespace Volatus.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelConfiguration.ApplyModelConfiguration(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is IEntity && (e.State == EntityState.Modified));

        foreach (EntityEntry entityEntry in entries)
            ((IEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

        return base.SaveChanges();
    }
}