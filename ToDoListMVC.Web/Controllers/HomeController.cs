using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoListMVC.Entity.ViewModels.TaskJobs;
using ToDoListMVC.Service.Services.Abstractions;
using ToDoListMVC.Web.Models;

namespace ToDoListMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskJobService taskJobService;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, ITaskJobService taskJobService, IMapper mapper)
        {
            _logger = logger;
            this.taskJobService = taskJobService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var taskJobs = await taskJobService.GetAllTaskJobAsync();
            var taskJobsMap = mapper.Map<List<TaskJobViewModel>>(taskJobs);
            return View(taskJobsMap);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
