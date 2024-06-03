using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ToDoListMVC.Service.Services.Abstractions;
using ToDoListMVC.Service.Services.Concretes;

namespace ToDoListMVC.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddScoped<ITaskJobService, TaskJobService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
