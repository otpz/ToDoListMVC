using AutoMapper;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Entity.ViewModels.Users;

namespace ToDoListMVC.Service.AutoMapper.Users
{
    public class UserMap : Profile
    {
        public UserMap()
        {
            CreateMap<AppUser, UserLoginViewModel>().ReverseMap();
            CreateMap<AppUser, UserRegisterViewModel>().ReverseMap();
            CreateMap<UserHeaderViewModel, AppUser>().ReverseMap();
        }
    }
}
