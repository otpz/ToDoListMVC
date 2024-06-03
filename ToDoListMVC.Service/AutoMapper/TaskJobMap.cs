using AutoMapper;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.TaskJobs;

namespace ToDoListMVC.Service.Mappings
{
    public class TaskJobMap : Profile
    {
        public TaskJobMap()
        {
            CreateMap<TaskJob, TaskJobViewModel>().ReverseMap();
        }
    }
}
