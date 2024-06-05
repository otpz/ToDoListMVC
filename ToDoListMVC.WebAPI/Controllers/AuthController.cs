using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            string result = await authService.LoginUserAsync(userLoginViewModel);
            if (result == "notfound")
            {
                return BadRequest(result);
            } 
            else if (result == "ok")
            {
                return Ok(result);
            } 
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            string result = await authService.RegisterUserAsync(userRegisterViewModel);
            if (result == "ok")
            {
                return Ok(result);
            }else
            {
                //ModelState.AddModelError("", "Bilgileri doğru giriniz");
                return BadRequest(result);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout() 
        { 
            try
            {
                await authService.LogoutUserAsync();
                var response = new { Success = true, Message = "Logout successful" };
                return Ok(response);
            } catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while logging out.", Details = ex.Message });
            }
        }

    }
}
