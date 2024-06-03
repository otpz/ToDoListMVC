using ToDoListMVC.Core.EntityBase;
namespace ToDoListMVC.Entity.Entities
{
    public class TaskJob : EntityBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsActive { get; set; } = true;
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
