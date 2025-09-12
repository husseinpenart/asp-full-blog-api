using Microsoft.AspNetCore.Mvc;
using myblog.extensions;
using myblog.models.DtoModels;
using myblog.services.auth;
using myblog.Services;
using System.Threading.Tasks;

namespace myblog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                var token = await _authService.RegisterAsync(userDto);
                return Ok(new ApiResponseExtension<object>
                {
                    Success = true,
                    
                    StatusCode = StatusCodes.Status200OK,
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}