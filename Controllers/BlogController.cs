using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myblog.extensions;
using myblog.models.DtoModels;
using myblog.services.blogs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace myblog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IblogService _blogService;

        public BlogController(IblogService blogService)
        {
            _blogService = blogService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _blogService.GetAllAsync(pageNumber, pageSize);
            if (!result.Success || !ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<PaginatedResponseDto<blogResponseDto>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                ItemLength = result.Data.TotalItems,
                Data = result.Data
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] blogCrudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<blogResponseDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input",
                    Data = null
                });

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _blogService.CreateAsync(dto, userId);

            if (!result.Success)
                return BadRequest(new ApiResponseExtension<blogResponseDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<blogResponseDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] blogCrudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<blogResponseDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid input",
                    Data = null
                });

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _blogService.UpdateAsync(id, dto, userId);

            if (!result.Success)
                return BadRequest(new ApiResponseExtension<blogResponseDto>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<blogResponseDto>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _blogService.DeleteAsync(id, userId);

            if (!result.Success)
                return BadRequest(new ApiResponseExtension<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = null
                });

            return Ok(new ApiResponseExtension<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = null
            });
        }
    }
}