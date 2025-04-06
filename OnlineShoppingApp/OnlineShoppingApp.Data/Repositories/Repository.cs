using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Data.Context;
using OnlineShoppingApp.Data.Entities;

namespace OnlineShoppingApp.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly OnlineShoppingAppDbContext _db;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(OnlineShoppingAppDbContext db)
    {
        _db = db;
        _dbSet = db.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        entity.CreatedDate = DateTime.Now;
        _dbSet.Add(entity);
    }

    public void Delete(TEntity entity, bool softDelete = true)
    {
        if (softDelete)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        else
        {
            _dbSet.Remove(entity);
        }
    }

    public void Delete(int id)
    {
        var entity = _dbSet.Find(id);
        Delete(entity);
    }

    public void Update(TEntity entity)
    {
        entity.ModifiedDate = DateTime.Now;
        _dbSet.Update(entity);
    }

    public TEntity GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public TEntity GetById(params object[] keyValues)
    {
        return _dbSet.Find(keyValues);
    }

    public TEntity Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
    {
        return predicate is null ? _dbSet : _dbSet.Where(predicate);
    }
}