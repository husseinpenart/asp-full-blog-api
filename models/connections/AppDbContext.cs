

using Microsoft.EntityFrameworkCore;
using myblog.models.Private.blog;
using myblog.models.Private.users;

namespace myblog.models.connections
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<blogModel> blogmodel { get; set; }
        public DbSet<userModel> Users { get; set; }
        
    }
}