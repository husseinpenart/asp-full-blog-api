using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myblog.models.DtoModels;
using myblog.models.Private.blog;

namespace myblog.services.global.post
{
    public interface IGlobalBlogServices
    {
        Task<(bool Success , string Message , PaginatedResponseDto<blogResponseDto> Data)>GetAllGlobalAsync(int pageNumber, int pageSize);
        Task <(bool Success , string Message , blogResponseDto Data)> GetBlogBySlugAsync(string slug);
    }
}