using ToDoListMVC.Core.EntityBase;
using ToDoListMVC.Data.Repositories.Abstractions;

namespace ToDoListMVC.Data.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T: class, IEntityBase, new();
        Task<int> SaveAsync();
        int Save();
    }
}
