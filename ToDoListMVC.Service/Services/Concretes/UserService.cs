using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ToDoListMVC.Data.UnitOfWorks;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Service.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task<AppUser> GetLoggedInUserAsync()
        {
            var userId = _user.GetLoggedInUserId();
            if (userId == null)
            {
                return null;
            }
            var user = await unitOfWork.GetRepository<AppUser>().GetByIdAsync(userId);
            return user;
        }

        public async Task<AppUser> GetUserProfileByIdAsync(int id)
        {
            var userById = await unitOfWork.GetRepository<AppUser>().GetByIdAsync(id);
            if (userById == null)
            {
                return null;
            }

            var user = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userById.Id, t => t.TaskJobs.OrderBy(x=>x.Priority).Where(x=> x.IsActive && !x.IsDeleted));
            return user;
        }
    }
}
