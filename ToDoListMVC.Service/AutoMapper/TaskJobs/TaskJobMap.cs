using AutoMapper;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.TaskJobs;

namespace ToDoListMVC.Service.AutoMapper.TaskJobs
{
    public class TaskJobMap : Profile
    {
        public TaskJobMap()
        {
            CreateMap<TaskJob, TaskJobViewModel>().ReverseMap();
            CreateMap<TaskJob, TaskJobAddViewModel>().ReverseMap();
            CreateMap<TaskJob, TaskJobListViewModel>().ReverseMap();
        }
    }
}
