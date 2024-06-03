using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Web.ViewComponents
{
    public class UserHeaderViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserHeaderViewComponent(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userService.GetLoggedInUserAsync();
            var userHeaderMap = mapper.Map<UserHeaderViewModel>(user);
            return View(userHeaderMap);
        }
    }
}
