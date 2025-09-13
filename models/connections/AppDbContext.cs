

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
            // Configure one-to-many relationship between userModel and blogModel
            modelBuilder.Entity<blogModel>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Deletes blogs if user is deleted

            // Ensure Email is unique in userModel
            modelBuilder.Entity<userModel>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}