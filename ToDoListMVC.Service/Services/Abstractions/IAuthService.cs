using ToDoListMVC.Entity.ViewModels.Users;

namespace ToDoListMVC.Service.Services.Abstractions
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(UserRegisterViewModel userRegisterViewModel);
        Task<string> LoginUserAsync(UserLoginViewModel userLoginViewModel);
        Task LogoutUserAsync();
    }
}
