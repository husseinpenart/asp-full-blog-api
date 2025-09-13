

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<blogModel>()
            .HasOne(a => a.User)
            .WithMany(a => a.Blogs)
            .HasForeignKey(a => a.User.Id)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<userModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}