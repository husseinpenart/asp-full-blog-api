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
        public async Task<(bool Success, string Message, List<blogResponseDto> Data)> GetAllAsync()
        {
            var blog = await _blogRepository.GetAllAsync();
            var response = blog.Select(p => new blogResponseDto
            {
                Id = p.Id,
                title = p.title,
                writer = p.writer,
                Description = p.Description,
                category = p.category,
                createdAt = p.createdAt
            }).ToList();
            return (true, "list of blogs", response);
        }
        // get by id 
        public async Task<(bool Success, string Message, blogResponseDto Data)> GetByIdAsync(Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return (false, "blog not found", null);
            }
            var respose = new blogResponseDto
            {
                Id = blog.Id,
                title = blog.title,
                writer = blog.writer,
                Description = blog.Description,
                createdAt = blog.createdAt,
                category = blog.category,
            };
            return (true, "blog by id found ", respose);
        }
        //create blog
        public async Task<(bool Success, string Message, blogResponseDto Data)> CreateAsync(blogCrudDto dto)
        {
            string relativePath = null;

            if (!string.IsNullOrEmpty(dto.cover))
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blog");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}.jpg"; // or detect extension
                var filePath = Path.Combine(uploadDir, fileName);

                // Decode Base64 string
                var imageBytes = Convert.FromBase64String(dto.cover);

                await File.WriteAllBytesAsync(filePath, imageBytes);

                relativePath = Path.Combine("uploads", "blog", fileName);
            }

            var blog = new blogModel
            {
                Id = Guid.NewGuid(),
                title = dto.title,
                Description = dto.Description,
                writer = dto.writer,
                category = dto.category,
                ImagePath = relativePath,

            };
            await _blogRepository.AddAsync(blog);
            var response = new blogResponseDto
            {

                Id = blog.Id,
                title = blog.title,
                writer = blog.writer,
                Description = blog.Description,
                createdAt = blog.createdAt,
                category = blog.category,
                ImagePath = blog.ImagePath,
            };
            return (true, $"blog with title of {blog.title} successfully ", response);
        }

        //update blog
        public async Task<(bool Success, string Message, blogResponseDto Data)> UpdateAsync(Guid id, blogResponseDto dto)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null) return (false, "blog not found", null);
            blog.title = dto.title;
            blog.Description = dto.Description;
            blog.writer = dto.writer;
            blog.category = dto.category;

            await _blogRepository.UpdateAsync(blog);
            var response = new blogResponseDto
            {
                Id = blog.Id,
                title = blog.title,
                writer = blog.writer,
                Description = blog.Description,
                createdAt = blog.createdAt,
                category = blog.category,
            };
            return (true, $"blog with id of {blog.Id} updated successfully", response);
        }

        // delete blog 
        public async Task<(bool Success, string Message)> DeleteAsync(Guid id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return (false, "blog not found");
            }
            await _blogRepository.DeleteAsync(id);
            return (false, "blog deleted Successfully");
        }
    }
}