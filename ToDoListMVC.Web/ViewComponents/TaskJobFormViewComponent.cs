using Microsoft.AspNetCore.Mvc;
using ToDoListMVC.Entity.ViewModels.TaskJobs;

namespace ToDoListMVC.Web.ViewComponents
{
    public class TaskJobFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TaskJobAddViewModel();
            return View(model);
        }
    }
}
