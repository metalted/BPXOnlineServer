using Metalted.BPX.Data;
using Microsoft.EntityFrameworkCore;

namespace Metalted.BPX.DataStore;

public interface IDatabase
{
    DbSet<TModel> GetDbSet<TModel>()
        where TModel : class;

    int SaveChanges();
}

public class Database : IDatabase
{
    private readonly BpxContext _db;

    public Database(BpxContext db)
    {
        _db = db;
        _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<TModel> GetDbSet<TModel>()
        where TModel : class
    {
        return _db.Set<TModel>();
    }

    public int SaveChanges()
    {
        return _db.SaveChanges();
    }
}
