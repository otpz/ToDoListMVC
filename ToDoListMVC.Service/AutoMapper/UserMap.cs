using AutoMapper;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;

namespace ToDoListMVC.Service.Mappings
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<AppUser, UserLoginViewModel>().ReverseMap();
            CreateMap<AppUser, UserRegisterViewModel>().ReverseMap();
        }
    }
}
