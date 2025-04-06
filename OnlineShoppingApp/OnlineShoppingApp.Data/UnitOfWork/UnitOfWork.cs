using Microsoft.EntityFrameworkCore.Storage;
using OnlineShoppingApp.Data.Context;

namespace OnlineShoppingApp.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly OnlineShoppingAppDbContext _db;
    private IDbContextTransaction _transaction;

    public UnitOfWork(OnlineShoppingAppDbContext db)
    {
        _db = db;
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task BeginTransaction()
    {
        _transaction = await _db.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransaction()
    {
        await _transaction.RollbackAsync();
    }
}