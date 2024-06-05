using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;

namespace ToDoListMVC.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<AppUser> GetLoggedInUserAsync();
        Task<AppUser> GetUserProfileWithTaskByIdAsync(int id);

        Task<AppUser> GetUserProfileWithDisabledTaskByIdAsync(int id);
        Task<AppUser> GetUserInfoAsync();
        Task<string> UpdateUserProfileAsync(UserSettingsViewModel userSettingsViewModel);

    }
}
