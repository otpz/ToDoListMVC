using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                string results = await authService.LoginUserAsync(userLoginViewModel);
                if (results == "notfound")
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı");
                    return View();
                }
                else if (results == "ok")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya şifreniz yanlıştır.");
                    return View();
                }
            }
            else
                return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            string result = await authService.RegisterUserAsync(userRegisterViewModel);
            if (result == "ok")
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ModelState.AddModelError("", "Bilgileri doğru giriniz");
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await authService.LogoutUserAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
