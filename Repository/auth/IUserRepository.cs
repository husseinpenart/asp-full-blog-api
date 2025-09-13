using Microsoft.EntityFrameworkCore;
using myblog.models.connections;
using myblog.models.Private.users;

namespace myblog.Repository.users
{
    public interface IUserRepository
    {
        Task AddAsync(userModel user);
        Task<userModel> GetByEmailAsync(string email);
        Task<userModel> GetByIdAsync(Guid id);
        Task UpdateAsync(userModel user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(userModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<userModel> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Blogs) // Include blogs for profile if needed
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<userModel> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Blogs) // Include blogs for profile if needed
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(userModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}