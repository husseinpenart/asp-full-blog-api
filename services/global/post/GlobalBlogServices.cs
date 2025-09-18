using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myblog.models.DtoModels;
using myblog.models.Private.blog;
using myblog.Repository.global.blog;

namespace myblog.services.global.post
{
    public class GlobalBlogServices : IGlobalBlogServices
    {
        private readonly IblogGlobalReposiotory _iblogGlobalReposiotory;
        public GlobalBlogServices(IblogGlobalReposiotory ib)
        {
            _iblogGlobalReposiotory = ib;

        }
        public async Task<(bool Success, string Message, PaginatedResponseDto<blogResponseDto> Data)> GetAllGlobalAsync(int pageNumber, int pageSize)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 10;
                var (blog, totalItems) = await _iblogGlobalReposiotory.GetAllGlobalAsync(pageNumber, pageSize);
                //map the dto
                var paginationDto = blog.Select(p => new blogResponseDto
                {
                    Id = p.Id,
                    slug = p.slug,
                    title = p.title,
                    ImagePath = p.ImagePath,
                    Description = p.Description,
                    category = p.category,
                    writer = p.writer,
                    UserId = p.UserId,
                    createdAt = p.createdAt
                }
                ).ToList();
                // Create paginated response
                var response = new PaginatedResponseDto<blogResponseDto>
                {
                    Data = paginationDto,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),

                };
                return (true, "pagination fetched successfull ", response);

            }
            catch (Exception ex)
            {

                return (false, $"Error retrieving blogs: {ex.Message}", null);

            }
        }

        //get by slug
        public async Task<(bool Success, string Message, blogResponseDto Data)> GetBlogBySlugAsync(string slug)
        {
            try
            {
                var blog = await _iblogGlobalReposiotory.GetBlogBySlugAsync(slug);
                if (blog == null)
                {
                    return (false, $"Data with {blog.slug} not found", null);
                }
                var response = new blogResponseDto
                {
                    slug = blog.slug,
                    title = blog.title,
                    writer = blog.writer,
                    ImagePath = blog.ImagePath,
                    Description = blog.Description,
                    UserId = blog.UserId,
                    createdAt = blog.createdAt,
                    category = blog.category,
                };
                return (true, $"blog with slug of {response.slug} found", response);

            }
            catch (Exception ex)
            {

                return (false, $"error found : {ex}", null);
            }
        }

    }
}