using ToDoListMVC.Data.UnitOfWorks;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Service.Services.Concretes
{
    public class TaskJobService : ITaskJobService
    {
        private readonly IUnitOfWork unitOfWork;

        public TaskJobService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<TaskJob>> GetAllTaskJobAsync()
        {
            var taskJobs = await unitOfWork.GetRepository<TaskJob>().GetAllAsync();
            taskJobs = taskJobs.OrderBy(x => x.Priority).ToList();
            return taskJobs;
        }
    }
}
