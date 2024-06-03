using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ToDoListMVC.Data.UnitOfWorks;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Service.Services.Concretes
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMapper mapper;

        public AuthService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<string> RegisterUserAsync(UserRegisterViewModel userRegisterViewModel)
        {
            var userMap = mapper.Map<AppUser>(userRegisterViewModel);
            userMap.UserName = userRegisterViewModel.Email;
            var result = await userManager.CreateAsync(userMap, string.IsNullOrEmpty(userRegisterViewModel.Password) ? "" : userRegisterViewModel.Password); // identity result 

            if (result.Succeeded)
            {
                return "ok";
            }
            else
            {
                return "notok";
            }
        }

        public async Task<string> LoginUserAsync(UserLoginViewModel userLoginViewModel)
        {
            var user = await userManager.FindByEmailAsync(userLoginViewModel.Email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, userLoginViewModel.Password, true, false);
                if (result.Succeeded)
                {
                    return "ok";
                }
                else
                {
                    return "notok";
                }
            }
            else
            {
                return "notfound";
            }
        }
        public async Task LogoutUserAsync()
        {
            await signInManager.SignOutAsync();
        }


    }
}
