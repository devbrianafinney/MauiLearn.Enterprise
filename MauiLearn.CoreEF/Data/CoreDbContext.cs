using MauiLearn.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiLearn.CoreEF.Data;

public partial class CoreDbContext : DbContext
{
    //modelBuilder. <== Go back and just wanna learn more & options
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
          : base(options)
    {
    }
 
    /*2/11/2026: ApplyTimestamps(), SaveChanges(), SaveChangesAsync
     * - Automatic DateCreated on insert 
     * - Automatic DateUpdated on update 
     * - Zero duplication 
     * - Zero mistakes */
    private void ApplyTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity<Guid>>();
        foreach (var entry in entries) {
            if (entry.State == EntityState.Added)
                entry.Entity.DateCreated = DateTime.UtcNow;
            entry.Entity.DateUpdated = DateTime.UtcNow;//gets updated everytime regardless
        }
    }
    public override int SaveChanges() 
    { 
        ApplyTimestamps(); 
        return base.SaveChanges(); 
    }


    public override Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(token);
    }
}

//public partial class AppDbContext : DbContext
//{
//    public AppDbContext(DbContextOptions<AppDbContext> options)
//        : base(options)
//    {
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);
//        ApplyBaseEntityConventions(modelBuilder);
//    }

//    private void ApplyBaseEntityConventions(ModelBuilder modelBuilder)
//    {
//        foreach (var entity in modelBuilder.Model.GetEntityTypes())
//        {
//            if (typeof(IBaseDateEntity).IsAssignableFrom(entity.ClrType))
//            {
//                modelBuilder.Entity(entity.ClrType)
//                    .Property<DateTime>("DateCreated")
//                    .ValueGeneratedOnAdd()
//                    .HasDefaultValueSql(GetProviderDateSql());

//                modelBuilder.Entity(entity.ClrType)
//                    .Property<DateTime>("DateUpdated")
//                    .ValueGeneratedOnAddOrUpdate()
//                    .HasDefaultValueSql(GetProviderDateSql());
//            }
//        }
//    }

//    private string? GetProviderDateSql()
//    {
//        var provider = Database.ProviderName;

//        if (provider.Contains("MauiLearnCoreSqlLocal"))
//            return "GETDATE()";

//        if (provider.Contains("Sqlite"))
//            return "CURRENT_TIMESTAMP";

//        return null;
//    }

//    public override int SaveChanges()
//    {
//        UpdateTimestamps();
//        return base.SaveChanges();
//    }

//    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
//    {
//        UpdateTimestamps();
//        return base.SaveChangesAsync(cancellationToken);
//    }

//    private void UpdateTimestamps()
//    {
//        var now = DateTime.UtcNow;
//        var entries = ChangeTracker.Entries()
//            .Where(e => e.Entity is IBaseDateEntity &&
//                        (e.State == EntityState.Added || e.State == EntityState.Modified));

//        foreach (var entry in entries)
//        {
//            //this IBaseEntity IS FROM CoreEF -- there IS one just like at Core.Models.IBaseDateEntity (on purpose), 
//            //still trying to figure out differences with the date mods with "shared" namespaces with return back out
//            var entity = (IBaseDateEntity)entry.Entity;
//            if (entry.State == EntityState.Added)
//                entity.DateCreated = now;
//            entity.DateUpdated = now;
//        }
//    }
//}