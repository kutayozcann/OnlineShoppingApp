namespace OnlineShoppingApp.Data.UnitOfWork;

public interface IUnitOfWork  : IDisposable
{
    Task<int> SaveChangesAsync(); //Returns how many saves it affected
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollbackTransaction();
}