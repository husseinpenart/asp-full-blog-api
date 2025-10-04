using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myblog.models.connections;
using myblog.models.Private.blog;

namespace myblog.Repository.blog
{
    public class BlogRepository : IblogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<blogModel> Blogs, int TotalItems)> GetAllAsync(
            int pageNumber,
            int pageSize,
            Guid? userId = null
        )
        {
            if (pageNumber < 1)
                pageNumber = 1;
            if (pageSize < 1)
                pageSize = 10;

            var query = _context.blogmodel.Include(b => b.User).AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(b => b.UserId == userId.Value);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(b => b.createdAt) // Newest first
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }

        public async Task<blogModel> GetByIdAsync(Guid id)
        {
            return await _context
                .blogmodel.Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);
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
