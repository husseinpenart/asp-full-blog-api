using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myblog.models.DtoModels;

namespace myblog.services.auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserDto userDto);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}