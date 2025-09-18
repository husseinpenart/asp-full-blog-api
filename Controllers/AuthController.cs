using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myblog.extensions;
using myblog.models.DtoModels;
using myblog.services.auth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace myblog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _userService;

        public UserController(IAuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<UserDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input",
                    Data = null
                });

            var result = await _userService.RegisterAsync(dto);
            if (!result.Success)
                return BadRequest(new ApiResponseExtension<UserDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<UserDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<string>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input",
                    Data = null
                });

            var result = await _userService.LoginAsync(dto);
            if (!result.Success)
                return BadRequest(new ApiResponseExtension<string>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<string>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Token
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userService.GetProfileAsync(userId);

            if (!result.Success)
                return BadRequest(new ApiResponseExtension<UserProfileDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<UserProfileDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<UserProfileDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input",
                    Data = null
                });

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _userService.UpdateProfileAsync(userId, dto);

            if (!result.Success)
                return BadRequest(new ApiResponseExtension<UserProfileDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<UserProfileDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }
    }
}