using Microsoft.EntityFrameworkCore;
using myblog.models.connections;
using myblog.models.Private.blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myblog.Repository.blog
{
    public class BlogRepository : IblogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<(List<blogModel> Items, int TotalItems)> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await _context.blogmodel.CountAsync();
            var items = await _context.blogmodel
                .OrderBy(b => b.createdAt) // Optional: Add sorting for consistency
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }

        public async Task<blogModel> GetByIdAsync(Guid id)
        {
            return await _context.blogmodel.FindAsync(id);
        }

        public async Task AddAsync(blogModel blog)
        {
            await _context.blogmodel.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(blogModel blog)
        {
            _context.blogmodel.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var blog = await _context.blogmodel.FindAsync(id);
            if (blog != null)
            {
                _context.blogmodel.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }
    }
}