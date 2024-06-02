using ToDoListMVC.Entity.Entities;

namespace ToDoListMVC.Service.Services.Abstractions
{
    public interface ITaskJobService
    {
        Task<List<TaskJob>> GetAllTaskJobAsync();
    }
}
