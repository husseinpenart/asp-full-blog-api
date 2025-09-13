using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using myblog.models.connections;
using myblog.models.DtoModels;
using myblog.models.Private.users;
using myblog.Repository.auth;
using myblog.services.auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace myblog.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<(bool Success, string Message, UserDto Data)> RegisterAsync(UserDto dto)
        {
            try
            {
                // Check if email already exists
                var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
                if (existingUser != null)
                    return (false, "Email already registered", null);

                var user = new userModel
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    phone = dto.Phone,
                    createdAt = DateTime.UtcNow
                    // Blogs is initialized as empty list by default
                };

                await _userRepository.AddAsync(user);

                var response = new UserDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.phone,
                    Password = user.Password,
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

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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
    }
}