using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myblog.models.DtoModels;

namespace myblog.services.blogs
{

    public interface IblogService
    {
        Task<(bool Success, string Message, PaginatedResponseDto<blogResponseDto> Data)> GetAllAsync(int pageNumber, int pageSize);
        Task<(bool Success, string Message, blogResponseDto Data)> GetByIdAsync(Guid id);
        Task<(bool Success, string Message, blogResponseDto Data)> CreateAsync(blogCrudDto dto);
        Task<(bool Success, string Message, blogResponseDto Data)> UpdateAsync(Guid id, blogResponseDto dto);
        Task<(bool Success, string Message)> DeleteAsync(Guid id);
    }

}