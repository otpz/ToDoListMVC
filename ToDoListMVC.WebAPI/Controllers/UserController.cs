using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListMVC.Entity.ViewModels.TaskJobs;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Helpers.PdfGenerator;
using ToDoListMVC.Service.Services.Abstractions;
using ToDoListMVC.Service.Services.Concretes;

namespace ToDoListMVC.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly ITaskJobService taskJobService;
        private readonly IPdfGenerator pdfGenerator;
        private readonly ClaimsPrincipal _user;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper, ITaskJobService taskJobService, IPdfGenerator pdfGenerator)
        {
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
            this.taskJobService = taskJobService;
            this.pdfGenerator = pdfGenerator;
            _user = httpContextAccessor.HttpContext.User;
        }

        [HttpGet("{userId?}")]
        public async Task<IActionResult> Index(int userId)
        {
            var loggedInUserId = _user.GetLoggedInUserId();
            if (loggedInUserId == 0)
            {
                return Unauthorized("Please login");
            }

            var user = await userService.GetUserProfileWithTaskByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userProfileMap = mapper.Map<UserProfileViewModel>(user);
            var response = new
            {
                UserProfile = userProfileMap,
                IsOwnProfile = loggedInUserId == user.Id
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskJobAddViewModel taskJobAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Görev bilgilerini tamamlayınız.");
                return BadRequest("Model state is not valid");
            }
            string title = await taskJobService.CreateTaskJobAsync(taskJobAddViewModel);
            return Ok(title);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userService.GetUserInfoAsync();
            var userSettingsMap = mapper.Map<UserSettingsViewModel>(user);
            return Ok(userSettingsMap);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserSettingsViewModel userSettingsViewModel)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (loggedInUserId != userSettingsViewModel.Id)
            {
                return Ok(loggedInUserId);
            }
            string email = await userService.UpdateUserProfileAsync(userSettingsViewModel);

            return Ok(loggedInUserId);
        }

        [HttpGet]
        public async Task<IActionResult> GeneratePdf()
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            string result = await pdfGenerator.GenerateUserDataPdfById(loggedInUserId);

            //return Ok(new {result, loggedInUserId});
            return Ok(new {loggedInUserId , result});
        }

        [HttpGet]
        public async Task<ActionResult> Update(int taskJobId)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (taskJobId == 0)
            {
                return NotFound(loggedInUserId);
            }
            string title = await taskJobService.DisableTaskJob(taskJobId);
            
            return Ok(new {result = title, loggedInUserId });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int taskJobId)
        {
            int loggedInUserId = _user.GetLoggedInUserId();
            if (taskJobId == 0)
            {
                return NotFound(loggedInUserId);
            }
            string title = await taskJobService.SafeDeleteTaskJob(taskJobId);

            return Ok(new { result = title, loggedInUserId });
        }
    }
}
