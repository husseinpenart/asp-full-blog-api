using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myblog.extensions;
using myblog.models.DtoModels;
using myblog.services.blogs;

namespace myblog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IblogService _blogService;

        public BlogController(IblogService blogService)
        {
            _blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(
                    new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "User not authenticated",
                        Data = null,
                    }
                );
            }

            var result = await _blogService.GetAllAsync(pageNumber, pageSize, userId);
            if (!result.Success || !ModelState.IsValid)
                return BadRequest(
                    new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = result.Message,
                        Data = null,
                    }
                );

            return Ok(
                new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    ItemLength = result.Data?.TotalItems ?? 0,
                    Data = result.Data,
                }
            );
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetAllPublic(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _blogService.GetAllAsync(pageNumber, pageSize);
            if (!result.Success || !ModelState.IsValid)
                return BadRequest(
                    new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = result.Message,
                        Data = null,
                    }
                );

            return Ok(
                new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    ItemLength = result.Data?.TotalItems ?? 0,
                    Data = result.Data,
                }
            );
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _blogService.GetByIdAsync(id);
            if (!result.Success || result.Data == null)
                return NotFound(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = result.Message,
                        Data = null,
                    }
                );

            return Ok(
                new ApiResponseExtension<blogResponseDto>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                }
            );
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] blogCrudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid input",
                        Data = null,
                    }
                );

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "User not authenticated",
                        Data = null,
                    }
                );
            }

            var result = await _blogService.CreateAsync(dto, userId);
            if (!result.Success)
                return BadRequest(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = result.Message,
                        Data = null,
                    }
                );

            return Ok(
                new ApiResponseExtension<blogResponseDto>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                }
            );
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] blogCrudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid input",
                        Data = null,
                    }
                );

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "User not authenticated",
                        Data = null,
                    }
                );
            }

            var result = await _blogService.UpdateAsync(id, dto, userId);
            if (!result.Success)
                return BadRequest(
                    new ApiResponseExtension<blogResponseDto>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = result.Message,
                        Data = null,
                    }
                );

            return Ok(
                new ApiResponseExtension<blogResponseDto>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = result.Data,
                }
            );
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized(
                    new ApiResponseExtension<object>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = "User not authenticated",
                        Data = null,
                    }
                );
            }

            var result = await _blogService.DeleteAsync(id, userId);
            if (!result.Success)
                return BadRequest(
                    new ApiResponseExtension<object>
                    {
                        Success = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = result.Message,
                        Data = null,
                    }
                );

            return Ok(
                new ApiResponseExtension<object>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = result.Message,
                    Data = null,
                }
            );
        }
    }
}
