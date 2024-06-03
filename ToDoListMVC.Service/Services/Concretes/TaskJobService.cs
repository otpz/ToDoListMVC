using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ToDoListMVC.Data.UnitOfWorks;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.TaskJobs;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Service.Services.Concretes
{
    public class TaskJobService : ITaskJobService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public TaskJobService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task<List<TaskJob>> GetAllTaskJobAsync()
        {
            var taskJobs = await unitOfWork.GetRepository<TaskJob>().GetAllAsync();
            taskJobs = taskJobs.OrderBy(x => x.Priority).ToList();
            return taskJobs;
        }

        public async Task<string> CreateTaskJobAsync(TaskJobAddViewModel taskJobAddViewModel)
        {
            int userId = _user.GetLoggedInUserId();
            if (userId == null)
            {
                return null;
            }
            var taskJob = new TaskJob
            {
                Title = taskJobAddViewModel.Title,
                Description = taskJobAddViewModel.Description,
                Priority = taskJobAddViewModel.Priority,
                UserId = userId
            };
            await unitOfWork.GetRepository<TaskJob>().AddAsync(taskJob);
            await unitOfWork.SaveAsync();
            return taskJob.Title;
        }

    }
}
