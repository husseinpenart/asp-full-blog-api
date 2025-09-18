using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myblog.models.Private.blog;

namespace myblog.Repository.global.blog
{
    public interface IblogGlobalReposiotory
    {
        Task<(List<blogModel> item, int TotalItems)> GetAllGlobalAsync(int page, int pageSize);
        Task<blogModel> GetBlogBySlugAsync(string slug);
    }
}