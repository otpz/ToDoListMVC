﻿using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.TaskJobs;

namespace ToDoListMVC.Service.Services.Abstractions
{
    public interface ITaskJobService
    {
        Task<List<TaskJob>> GetAllTaskJobAsync();
        Task<string> CreateTaskJobAsync(TaskJobAddViewModel taskJobAddViewModel);
        Task<string> DisableTaskJob(int taskJobId);
        Task<string> SafeDeleteTaskJob(int taskJobId);
    }
}
