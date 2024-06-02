using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoListMVC.Entity.Entities;

namespace ToDoListMVC.Data.Configurations
{
    public class TaskMap : IEntityTypeConfiguration<TaskJob>
    {
        public void Configure(EntityTypeBuilder<TaskJob> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(100);
                
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.HasData(
            new TaskJob
            {
                Id = 1,
                Title = "Ödevi Yap",
                Description = "Elektrik Makinaları 2 ödevi var.",
                CreatedDate = DateTime.Now,
                DeletedDate = null,
                IsActive = true,
                Priority = 1,
                IsDeleted = false,
            },
            new TaskJob
            {
                Id = 2,
                Title = "Staj defterini tamamla",
                Description = "Son haftanın staj defterinde eksikler var.",
                CreatedDate = DateTime.Now,
                DeletedDate = null,
                IsActive = true,
                Priority = 2,
                IsDeleted = false,
            },
            new TaskJob
            {
                Id = 3,
                Title = "OBS'yi kontrol et",
                Description = "OBS'den bir mesaj gelebilir.",
                CreatedDate = DateTime.Now,
                DeletedDate = null,
                IsActive = true,
                Priority = 3,
                IsDeleted = false,
            });
        }
    }
}
