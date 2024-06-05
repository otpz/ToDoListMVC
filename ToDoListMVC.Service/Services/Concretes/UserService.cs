using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ToDoListMVC.Data.UnitOfWorks;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Service.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly ClaimsPrincipal _user;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.userManager = userManager;
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

        public async Task<AppUser> GetUserInfoAsync()
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            var user = await unitOfWork.GetRepository<AppUser>().GetByIdAsync(loggedInUserId);
            return user;
        }

        public async Task<AppUser> GetUserProfileWithDisabledTaskByIdAsync(int id)
        {
            var userById = await unitOfWork.GetRepository<AppUser>().GetByIdAsync(id);
            if (userById == null)
            {
                return null;
            }

            var user = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userById.Id, t => t.TaskJobs.OrderBy(x => x.Priority).Where(x => !x.IsActive && !x.IsDeleted));
            return user;
        }

        public async Task<AppUser> GetUserProfileWithTaskByIdAsync(int id)
        {
            var userById = await unitOfWork.GetRepository<AppUser>().GetByIdAsync(id);
            if (userById == null)
            {
                return null;
            }

            var user = await unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == userById.Id, t => t.TaskJobs.OrderBy(x=>x.Priority).Where(x=> x.IsActive && !x.IsDeleted));
            return user;
        }
        public async Task<string> UpdateUserProfileAsync(UserSettingsViewModel userSettingsViewModel)
        {
            if (userSettingsViewModel.Id != _user.GetLoggedInUserId())
            {
                return null;
            }

            var user = await unitOfWork.GetRepository<AppUser>().GetByIdAsync(userSettingsViewModel.Id);

            user.FirstName = userSettingsViewModel.FirstName;
            user.LastName = userSettingsViewModel.LastName;
            user.Biography = userSettingsViewModel.Biography;
                
            await userManager.UpdateAsync(user);

            return user.Email;
        }
    }
}
