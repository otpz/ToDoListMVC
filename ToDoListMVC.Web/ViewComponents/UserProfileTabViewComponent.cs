using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoListMVC.Entity.ViewModels.Users;
using ToDoListMVC.Service.Extensions;

namespace ToDoListMVC.Web.ViewComponents
{
    public class UserProfileTabViewComponent : ViewComponent
    {
        private readonly ClaimsPrincipal _user;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserProfileTabViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _user.GetLoggedInUserId();
            var model = new UserTabViewModel { Id = userId };
            return View(model);
        }
    }
}
