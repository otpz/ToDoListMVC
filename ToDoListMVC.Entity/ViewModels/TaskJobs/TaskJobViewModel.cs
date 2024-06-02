namespace ToDoListMVC.Entity.ViewModels.TaskJobs
{
    public class TaskJobViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Priority { get; set; }
    }
}
