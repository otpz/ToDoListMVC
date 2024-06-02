namespace ToDoListMVC.Core.EntityBase
{
    public class EntityBase : IEntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set;} = null;
        public bool IsDeleted { get; set; } = false;
    }
}
