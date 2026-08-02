using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OmniPulse.Entities.Common;
using OmniPulse.Entities.Models;

namespace OmniPulse.DataAccess; 

public class OmniPulseDbContext : DbContext
{
    public OmniPulseDbContext(DbContextOptions<OmniPulseDbContext> options) : base(options) { }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityTypes = typeof(BaseEntity).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(BaseEntity)) && !t.IsAbstract);

        foreach (var type in entityTypes) { modelBuilder.Entity(type); }

base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added) {
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.IsActive = true;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}