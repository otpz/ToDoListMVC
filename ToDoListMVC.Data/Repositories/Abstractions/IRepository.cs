using ToDoListMVC.Core.EntityBase;

namespace ToDoListMVC.Data.Repositories.Abstractions
{
    public interface IRepository<T> where T : class, IEntityBase, new()
    {
    }
}
