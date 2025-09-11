

using Microsoft.EntityFrameworkCore;
using myblog.models.Private.blog;

namespace myblog.models.connections
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<blogModel> blogmodel { get; set; }
        
    }
}