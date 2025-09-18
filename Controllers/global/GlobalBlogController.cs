using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myblog.extensions;
using myblog.models.DtoModels;
using myblog.services.global.post;

namespace myblog.Controllers.global
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GlobalBlogController : ControllerBase
    {
        private readonly IGlobalBlogServices _globalBlogServices;
        public GlobalBlogController(IGlobalBlogServices gl)
        {
            _globalBlogServices = gl ?? throw new ArgumentNullException(nameof(gl));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _globalBlogServices.GetAllGlobalAsync(pageNumber, pageSize);
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
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetGlobalBlogAsync(string slug)
        {

            var result = await _globalBlogServices.GetBlogBySlugAsync(slug);
            if (!result.Success)
                return NotFound(new { message = result.Message });
            return Ok(result.Data);
        }

    }
}