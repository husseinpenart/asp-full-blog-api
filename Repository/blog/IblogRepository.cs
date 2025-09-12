using myblog.models.Private.blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myblog.Repository.blog
{
    public interface IblogRepository
    {
        Task<(List<blogModel> Items, int TotalItems)> GetAllAsync(int pageNumber, int pageSize);
        Task<blogModel> GetByIdAsync(Guid id);
        Task AddAsync(blogModel blog);
        Task UpdateAsync(blogModel blog);
        Task DeleteAsync(Guid id);
    }
}