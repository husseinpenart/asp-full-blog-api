
using Microsoft.EntityFrameworkCore;
using myblog.models.connections;
using myblog.models.Private.blog;

namespace myblog.Repository.blog
{
    public class blogRepository : IblogRepository
    {
        private readonly AppDbContext _context;
        public blogRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<blogModel>> GetAllAsync()
        {
            return await _context.blogmodel.ToListAsync();
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