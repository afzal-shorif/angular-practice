﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Dtos;
using Entity = UserManagement.Core.Entities;

namespace UserManagement.Application.Services.User
{
    public interface IUserService
    {
        Task<ApiResponse<object>> RegisterUser(UserRegistrationDto userRegDto, string userRole);
        Task<ApiResponse<object>> GetUsers(ClaimsPrincipal claimsPrincipal);
        Task<ApiResponse<object>> GetCurrentUserInfo(ClaimsPrincipal claimsPrincipal);
        Task<ApiResponse<object>> UpdateUserInfoAsync(Entity.User user, UpdateUserInfoDto userInfoDto);
    }
}
