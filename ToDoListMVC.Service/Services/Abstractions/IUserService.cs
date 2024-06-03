﻿using ToDoListMVC.Entity.Entities;

namespace ToDoListMVC.Service.Services.Abstractions
{
    public interface IUserService
    {
        Task<AppUser> GetLoggedInUserAsync();
    }
}