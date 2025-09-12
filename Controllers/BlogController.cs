
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myblog.extensions;
using myblog.models.DtoModels;
using myblog.services.blogs;

namespace myblog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IblogService _blogService;

        public BlogController(IblogService blogservice)
        {

            _blogService = blogservice;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _blogService.GetAllAsync();
            if (!ModelState.IsValid)
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
                ItemLength = result.Data.Count,
                Data = result.Data
            });
        }
        // create post 
        [Authorize]
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] blogCrudDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<object>
                {
                    Success = false,
                    Message = "Invalid response",
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null
                });

            var result = await _blogService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new ApiResponseExtension<object>
                {
                    Success = false,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null
                });

            return Ok(new ApiResponseExtension<object>
            {
                Success = true,
                Message = result.Message,
                StatusCode = StatusCodes.Status200OK,
                Data = result.Data
            });
        }
        // get by id 
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _blogService.GetByIdAsync(id);
            if (!result.Success && result.Data == null)
                return BadRequest(new ApiResponseExtension<object>
                {
                    Success = false,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null

                });
            return Ok(new ApiResponseExtension<object>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = result.Message,
                Data = result.Data
            });
        }
        // update by id

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] blogResponseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponseExtension<object>
                {
                    Success = false,
                    Message = "Respose Error",
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null
                });
            var result = await _blogService.UpdateAsync(id, dto);
            return Ok(new ApiResponseExtension<object>
            {
                Success = true,
                Message = result.Message,
                StatusCode = StatusCodes.Status200OK,
                Data = result.Data
            });

        }

        // deleting the blog 
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var result = await _blogService.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(new ApiResponseExtension<object>
                {
                    Success = false,
                    Message = result.Message,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Data = null
                });
            return Ok(new ApiResponseExtension<object>
            {
                Success = true,
                Message = result.Message,
                StatusCode = StatusCodes.Status200OK,
                Data = null
            });
        }

    }
}