using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public AuthController(IAuthService authService, IMapper mapper, IToastNotification toastNotification, IHttpContextAccessor httpContextAccessor)
        {
            this.authService = authService;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
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
                    toastNotification.AddErrorToastMessage("Kullanıcı bulunamadı", new ToastrOptions { Title = "Hata" });
                    return View();
                }
                else if (results == "ok")
                {
                    toastNotification.AddSuccessToastMessage("Giriş başarılı", new ToastrOptions { Title = "Başarılı" });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya şifreniz yanlıştır.");
                    toastNotification.AddErrorToastMessage("E-posta veya şifreniz yanlıştır.", new ToastrOptions { Title = "Hata" });
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
                toastNotification.AddErrorToastMessage("Kayıt başarıyla oluşturuldu.", new ToastrOptions { Title = "Başarılı" });
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                toastNotification.AddErrorToastMessage("Bilgileri doğru giriniz", new ToastrOptions { Title = "Hata" });
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
