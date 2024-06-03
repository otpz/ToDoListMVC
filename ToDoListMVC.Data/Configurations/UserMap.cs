using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoListMVC.Entity.Entities;

namespace ToDoListMVC.Data.Configurations
{
    public class UserMap : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Indexes for "normalized" username and email, to allow efficient lookups
            builder.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");

            // Maps to the AspNetUsers table
            builder.ToTable("AspNetUsers");

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.UserName).HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Property(u => u.Email).HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail).HasMaxLength(256);

            // The relationships between User and other entity types
            // Note that these relationships are configured with no navigation properties

            //// Each User can have many UserClaims
            //builder.HasMany<IdentityUserClaim<int>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

            //// Each User can have many UserLogins
            //builder.HasMany<IdentityUserLogin<int>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

            //// Each User can have many UserTokens
            //builder.HasMany<IdentityUserToken<int>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

            //// Each User can have many entries in the UserRole join table
            //builder.HasMany<IdentityUserRole<int>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();

            var user = new AppUser
            {
                Id = 1,
                FirstName = "User",
                LastName = "Test",
                Email = "test@gmail.com",
                NormalizedEmail = "TEST@GMAIL.COM",
                UserName = "test@gmail.com",
                NormalizedUserName = "TEST@GMAIL.COM",
                PhoneNumber = "1234567890",
                SecurityStamp = "12345678901",
            };
            user.PasswordHash = CreatePasswordHash(user, "123123asdA.");

            var user2 = new AppUser
            {
                Id = 2,
                FirstName = "User2",
                LastName = "Test",
                Email = "test2@gmail.com",
                NormalizedEmail = "TEST2@GMAIL.COM",
                UserName = "test2@gmail.com",
                NormalizedUserName = "TEST2@GMAIL.COM",
                PhoneNumber = "1234567890",
                SecurityStamp = "12345678902",
            };
            user2.PasswordHash = CreatePasswordHash(user2, "123123asdA.");

            builder.HasData(user, user2);
        }
        private string CreatePasswordHash(AppUser user, string password)
        {
            var passwordHasher = new PasswordHasher<AppUser>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}
