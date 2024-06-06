using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.TaskJobs;

namespace ToDoListMVC.Entity.ViewModels.Users
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IList<TaskJobListViewModel> TaskJobs { get; set; }
    }
}
