using Microsoft.AspNetCore.Identity;
using ToDoListMVC.Core.EntityBase;

namespace ToDoListMVC.Entity.Entities
{
    public class AppUser : IdentityUser<string>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Biography { get; set; }
        public ICollection<TaskJob> TaskJobs { get; set; } = new List<TaskJob>();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; } = null;
        public bool IsDelete { get; set; } = false;
    }
}
