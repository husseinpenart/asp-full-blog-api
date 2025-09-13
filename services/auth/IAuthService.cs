using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myblog.models.DtoModels;

namespace myblog.services.auth
{
    public interface IAuthService
    {

        Task<(bool Success, string Message, UserDto Data)> RegisterAsync(UserDto dto);
        Task<(bool Success, string Message, string Token)> LoginAsync(LoginDto dto);
        Task<(bool Success, string Message, UserProfileDto Data)> GetProfileAsync(Guid userId);
        Task<(bool Success, string Message, UserProfileDto Data)> UpdateProfileAsync(Guid userId, UpdateUserDto dto);
    }
}