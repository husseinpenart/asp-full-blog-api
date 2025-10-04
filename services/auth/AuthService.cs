using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myblog.models.connections;
using myblog.models.DtoModels;
using myblog.models.Private.users;
using myblog.Repository.users;
using myblog.services.auth;

namespace myblog.services.users
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            AppDbContext context,
            IConfiguration configuration
        )
        {
            _userRepository =
                userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // RegisterAsync and LoginAsync remain unchanged
        public async Task<(bool Success, string Message, UserDto Data)> RegisterAsync(UserDto dto)
        {
            try
            {
                // Explicitly validate DTO to ensure Password is not null or empty
                if (
                    dto == null
                    || string.IsNullOrWhiteSpace(dto.Name)
                    || string.IsNullOrWhiteSpace(dto.Email)
                    || string.IsNullOrWhiteSpace(dto.Password)
                )
                {
                    return (false, "Name, email, and password are required", null);
                }

                if (dto.Password.Length < 8)
                {
                    return (false, "Password must be at least 8 characters", null);
                }

                if (!IsValidEmail(dto.Email))
                {
                    return (false, "Invalid email format", null);
                }

                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null)
                {
                    return (false, "Email already registered", null);
                }

                var user = new userModel
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    phone = dto.Phone,
                    createdAt = DateTime.UtcNow,
                };

                await _userRepository.AddAsync(user);

                var response = new UserDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.phone,
                };

                return (true, "User registered successfully", response);
            }
            catch (Exception ex)
            {
                return (false, $"Error registering user: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, string Token)> LoginAsync(LoginDto dto)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(dto.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return (false, "Invalid email or password", null);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Email, user.Email),
                        }
                    ),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return (true, "Login successful", tokenString);
            }
            catch (Exception ex)
            {
                return (false, $"Error logging in: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, UserProfileDto Data)> GetProfileAsync(
            Guid userId
        )
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return (false, "User not found", null);

                var response = new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.phone,
                    CreatedAt = user.createdAt,
                    Blogs =
                        user.Blogs?.Select(b => new blogResponseDto
                            {
                                Id = b.Id,
                                title = b.title,
                                ImagePath = b.ImagePath,
                                Description = b.Description,
                                category = b.category,
                                writer = b.writer,
                                UserId = b.UserId,
                                createdAt = b.createdAt,
                            })
                            .ToList() ?? new List<blogResponseDto>(),
                };

                return (true, "Profile retrieved successfully", response);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving profile: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, UserProfileDto Data)> UpdateProfileAsync(
            Guid userId,
            UpdateUserDto dto
        )
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return (false, "User not found", null);

                bool isUpdated = false;

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    user.Name = dto.Name;
                    isUpdated = true;
                }

                if (!string.IsNullOrWhiteSpace(dto.Email))
                {
                    if (!IsValidEmail(dto.Email))
                        return (false, "Invalid email format", null);

                    var existingUser = await _context.Users.FirstOrDefaultAsync(u =>
                        u.Email == dto.Email && u.Id != userId
                    );
                    if (existingUser != null)
                        return (false, "Email already registered", null);

                    user.Email = dto.Email;
                    isUpdated = true;
                }

                if (!string.IsNullOrWhiteSpace(dto.Phone))
                {
                    user.phone = dto.Phone;
                    isUpdated = true;
                }

                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    if (dto.Password.Length < 8)
                        return (false, "Password must be at least 8 characters", null);
                    user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                    isUpdated = true;
                }

                if (!isUpdated)
                    return (false, "No changes provided", null);

                await _userRepository.UpdateAsync(user);

                var response = new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.phone,
                    CreatedAt = user.createdAt,
                    Blogs =
                        user.Blogs?.Select(b => new blogResponseDto
                            {
                                Id = b.Id,
                                title = b.title,
                                ImagePath = b.ImagePath,
                                Description = b.Description,
                                category = b.category,
                                writer = b.writer,
                                UserId = b.UserId,
                                createdAt = b.createdAt,
                            })
                            .ToList() ?? new List<blogResponseDto>(),
                };

                return (true, "Profile updated successfully", response);
            }
            catch (Exception ex)
            {
                return (false, $"Error updating profile: {ex.Message}", null);
            }
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
