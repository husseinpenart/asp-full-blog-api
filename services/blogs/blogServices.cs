using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myblog.models.DtoModels;
using myblog.models.Private.blog;
using myblog.Repository.blog;

namespace myblog.services.blogs
{
    public class blogServices : IblogService
    {
        // call repository for extra function and connection rep
        private readonly IblogRepository _blogRepository;

        public blogServices(IblogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        // get all blogs
        public async Task<(
            bool Success,
            string Message,
            PaginatedResponseDto<blogResponseDto> Data
        )> GetAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1)
                    pageNumber = 1;
                if (pageSize < 1)
                    pageSize = 10;
                if (pageSize > 100)
                    pageSize = 100; // Optional: Limit max page size

                // Get paginated blogs and total count from repository
                var (blogs, totalItems) = await _blogRepository.GetAllAsync(pageNumber, pageSize);

                // Map to DTO
                var paginatedBlogs = blogs
                    .Select(p => new blogResponseDto
                    {
                        Id = p.Id,
                        slug = p.slug,
                        title = p.title,
                        ImagePath = p.ImagePath,
                        Description = p.Description,
                        category = p.category,
                        writer = p.User.Name,
                        UserId = p.UserId,
                        createdAt = p.createdAt,
                    })
                    .ToList();

                // Create paginated response
                var response = new PaginatedResponseDto<blogResponseDto>
                {
                    Data = paginatedBlogs,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                };

                return (true, "Paginated list of blogs", response);
            }
            catch (Exception ex)
            {
                // Handle exceptions and ensure a return value
                return (false, $"Error retrieving blogs: {ex.Message}", null);
            }
        }

        // get by id
        public async Task<(bool Success, string Message, blogResponseDto Data)> GetByIdAsync(
            Guid id
        )
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return (false, "blog not found", null);
            }
            var respose = new blogResponseDto
            {
                Id = blog.Id,
                slug = blog.slug,
                title = blog.title,
                writer = blog.writer,
                ImagePath = blog.ImagePath,
                Description = blog.Description,
                UserId = blog.UserId,
                createdAt = blog.createdAt,
                category = blog.category,
            };
            return (true, "blog by id found ", respose);
        }

        //create blog
        public async Task<(bool Success, string Message, blogResponseDto Data)> CreateAsync(
            blogCrudDto dto,
            Guid userId
        )
        {
            if (dto.cover == null || dto.cover.Length == 0)
                return (false, "Cover image is required for creating a blog", null);

            string relativePath = null;

            if (dto.cover != null && dto.cover.Length > 0)
            {
                var uploadDir = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Uploads",
                    "blog"
                );

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileExtension = Path.GetExtension(dto.cover.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.cover.CopyToAsync(stream);
                }

                relativePath = Path.Combine("uploads", "blog", fileName).Replace("\\", "/");
            }

            var blog = new blogModel
            {
                Id = Guid.NewGuid(),
                slug = dto.slug,
                title = dto.title,
                Description = dto.Description,
                writer = dto.writer ?? "Anonymous",
                category = dto.category,
                ImagePath = relativePath,
                UserId = userId,
                createdAt = DateTime.UtcNow,
            };

            await _blogRepository.AddAsync(blog);

            var response = new blogResponseDto
            {
                Id = blog.Id,
                slug = blog.slug,
                title = blog.title,
                writer = blog.writer,
                Description = blog.Description,
                createdAt = blog.createdAt,
                category = blog.category,
                UserId = blog.UserId,
                ImagePath = blog.ImagePath,
            };

            return (true, $"Blog with title '{blog.title}' created successfully", response);
        }

        //update blog
        public async Task<(bool Success, string Message, blogResponseDto Data)> UpdateAsync(
            Guid id,
            blogCrudDto dto,
            Guid userId
        )
        {
            try
            {
                var blog = await _blogRepository.GetByIdAsync(id);
                if (blog == null)
                    return (false, "Blog not found", null);

                if (blog.UserId != userId)
                    return (false, "Unauthorized: You can only edit your own blogs", null);

                blog.title = dto.title;
                blog.slug = dto.slug;
                blog.Description = dto.Description;
                blog.category = dto.category;
                blog.writer = dto.writer ?? blog.writer;

                // Handle file upload if provided
                if (dto.cover != null && dto.cover.Length > 0)
                {
                    var uploadDir = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "Uploads",
                        "blog"
                    );

                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var fileExtension = Path.GetExtension(dto.cover.FileName);
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.cover.CopyToAsync(stream);
                    }

                    blog.ImagePath = Path.Combine("uploads", "blog", fileName).Replace("\\", "/");
                }
                // If no cover is provided, keep the existing ImagePath (no change needed)

                await _blogRepository.UpdateAsync(blog);

                var response = new blogResponseDto
                {
                    Id = blog.Id,
                    slug = blog.slug,
                    title = blog.title,
                    ImagePath = blog.ImagePath,
                    Description = blog.Description,
                    category = blog.category,
                    writer = blog.writer,
                    UserId = blog.UserId,
                    createdAt = blog.createdAt,
                };

                return (true, "Blog updated successfully", response);
            }
            catch (Exception ex)
            {
                return (false, $"Error updating blog: {ex.Message}", null);
            }
        }

        // delete blog
        public async Task<(bool Success, string Message)> DeleteAsync(Guid id, Guid userId)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return (false, "blog not found");
            }
            if (blog.UserId != userId)
                return (false, "Unauthorized: You can only delete your own blogs");
            await _blogRepository.DeleteAsync(id);
            return (false, "blog deleted Successfully");
        }
    }
}
