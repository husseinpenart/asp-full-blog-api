using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using myblog.models.Private.blog;

namespace myblog.Repository.blog
{
    public interface IblogRepository
    {
        Task<(IEnumerable<blogModel> Blogs, int TotalItems)> GetAllAsync(
            int pageNumber,
            int pageSize,
            Guid? userId = null
        );
        Task<blogModel> GetByIdAsync(Guid id);
        Task AddAsync(blogModel blog);
        Task UpdateAsync(blogModel blog);
        Task DeleteAsync(Guid id);
    }
}
