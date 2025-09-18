using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myblog.models.connections;
using myblog.models.Private.blog;

namespace myblog.Repository.global.blog
{
    public class blogGlobalReposiotory :IblogGlobalReposiotory
    {
        private readonly AppDbContext _context;
        public blogGlobalReposiotory(AppDbContext context)
        {
            _context = context;
        }
        public async Task<(List<blogModel> item, int TotalItems)> GetAllGlobalAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            var totalItems = await _context.blogmodel.CountAsync();
            var items = await _context.blogmodel
            .OrderBy(b => b.createdAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            return (items, totalItems);
        }
        public async Task<blogModel> GetBlogBySlugAsync(string slug)
        {
            var item = await _context.blogmodel
            .FirstOrDefaultAsync(b => b.slug == slug);
            return item;
        }

    }
}