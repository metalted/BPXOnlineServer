using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Metalted.BPX.DataStore;

public interface IBasicRepository<TModel>
    where TModel : class, IEntity
{
    int Count();
    int Count(Expression<Func<TModel, bool>> predicate);
    bool Exists(Expression<Func<TModel, bool>> predicate);
    TModel? GetById(int id);
    TModel? GetById(int id, Func<DbSet<TModel>, IQueryable<TModel>> set);
    IEnumerable<TModel> GetAll();
    IEnumerable<TModel> GetAll(Func<DbSet<TModel>, IQueryable<TModel>> set);
    IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> predicate);
    IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> predicate, Func<DbSet<TModel>, IQueryable<TModel>> set);
    TModel? GetSingle(Expression<Func<TModel, bool>> predicate);
    TModel? GetSingle(Expression<Func<TModel, bool>> predicate, Func<DbSet<TModel>, IQueryable<TModel>> set);
    TModel Insert(TModel model);
    TModel Insert(TModel model, DateTime dateCreated);
    TModel Update(TModel model);

    TModel Upsert(
        int id,
        Func<TModel> insert,
        Func<TModel, TModel> update);

    TModel Upsert(
        Expression<Func<TModel, bool>> predicate,
        Func<TModel> insert,
        Func<TModel, TModel> update);

    bool Delete(TModel model);
}

public abstract class BasicRepository<TModel> : IBasicRepository<TModel>
    where TModel : class, IEntity
{
    private readonly IDatabase _database;
    private readonly DbSet<TModel> _set;
    private readonly ILogger _logger;

    protected BasicRepository(IDatabase database, ILogger logger)
    {
        _database = database;
        _logger = logger;
        _set = _database.GetDbSet<TModel>();
    }

    public int Count()
    {
        return _set.Count();
    }

    public int Count(Expression<Func<TModel, bool>> predicate)
    {
        return _set.Count(predicate);
    }

    public bool Exists(Expression<Func<TModel, bool>> predicate)
    {
        return _set.Any(predicate);
    }

    public TModel? GetById(int id)
    {
        return _set.SingleOrDefault(x => x.Id == id);
    }

    public TModel? GetById(int id, Func<DbSet<TModel>, IQueryable<TModel>> set)
    {
        return set.Invoke(_set).SingleOrDefault(x => x.Id == id);
    }

    public IEnumerable<TModel> GetAll()
    {
        return _set;
    }

    public IEnumerable<TModel> GetAll(Func<DbSet<TModel>, IQueryable<TModel>> set)
    {
        return set.Invoke(_set);
    }

    public IEnumerable<TModel> GetAll(
        Expression<Func<TModel, bool>> predicate,
        Func<DbSet<TModel>, IQueryable<TModel>> set)
    {
        return set.Invoke(_set).Where(predicate);
    }

    public IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> predicate)
    {
        return _set.Where(predicate);
    }

    public TModel? GetSingle(Expression<Func<TModel, bool>> predicate)
    {
        return _set.FirstOrDefault(predicate);
    }

    public TModel? GetSingle(Expression<Func<TModel, bool>> predicate, Func<DbSet<TModel>, IQueryable<TModel>> set)
    {
        return set.Invoke(_set).FirstOrDefault(predicate);
    }

    public TModel Insert(TModel model)
    {
        return Insert(model, DateTime.UtcNow);
    }

    public TModel Insert(TModel model, DateTime dateCreated)
    {
        model.DateCreated = dateCreated;
        EntityEntry<TModel> entry = _set.Add(model);
        _database.SaveChanges();
        entry.State = EntityState.Detached;
        return entry.Entity;
    }

    public TModel Update(TModel model)
    {
        model.DateUpdated = DateTime.UtcNow;
        EntityEntry<TModel> entry = _set.Update(model);
        _database.SaveChanges();
        entry.State = EntityState.Detached;
        return entry.Entity;
    }

    public TModel Upsert(
        int id,
        Func<TModel> insert,
        Func<TModel, TModel> update)
    {
        TModel? model = GetById(id);
        return model == null
            ? Insert(insert.Invoke())
            : Update(update(model));
    }

    public TModel Upsert(
        Expression<Func<TModel, bool>> predicate,
        Func<TModel> insert,
        Func<TModel, TModel> update)
    {
        TModel? model = GetSingle(predicate);
        return model == null
            ? Insert(insert.Invoke())
            : Update(update(model));
    }

    public bool Delete(TModel model)
    {
        _set.Remove(model);
        int changes = _database.SaveChanges();
        return changes > 0;
    }
}
