using ToDoListMVC.Core.EntityBase;
using ToDoListMVC.Data.Repositories.Abstractions;

namespace ToDoListMVC.Data.Repositories.Concretes
{
    public class Repository<T> : IRepository<T> where T: class, IEntityBase, new()
    {

    }
}
