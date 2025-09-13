using System;
using System.Threading.Tasks;
using myblog.models.DtoModels;

namespace myblog.services.blogs
{
    public interface IblogService
    {
        Task<(bool Success, string Message, PaginatedResponseDto<blogResponseDto> Data)> GetAllAsync(int pageNumber, int pageSize);
        Task<(bool Success, string Message, blogResponseDto Data)> GetByIdAsync(Guid id);
        Task<(bool Success, string Message, blogResponseDto Data)> CreateAsync(blogCrudDto dto, Guid userId);
        Task<(bool Success, string Message, blogResponseDto Data)> UpdateAsync(Guid id, blogCrudDto dto, Guid userId);
        Task<(bool Success, string Message)> DeleteAsync(Guid id, Guid userId);
    }
}