using Gym.Data;
using Gym.DTO;
using Gym.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gym.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UserService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UserProfileDto> RegisterAsync(RegisterUserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already registered.");

            var role = string.IsNullOrWhiteSpace(dto.Role) ? "Member" : dto.Role.Trim();

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = role,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.BirthDate,
                Address = dto.Address,

                // ✅ Trainers need approval; Members get auto-approved
                IsApproved = role == "Trainer" ? false : true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToProfile(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            // ✅ Check approval before issuing token
            if (!user.IsApproved)
                throw new Exception("Your account has not been approved yet. Please wait for admin approval.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireDays = int.Parse(_config["Jwt:ExpireDays"] ?? "365");
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expireDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserProfileDto?> GetProfileAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user == null ? null : MapToProfile(user);
        }

        public async Task<UserProfileDto?> UpdateProfileAsync(string email, UpdateProfileDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            user.FullName = dto.FullName ?? user.FullName;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;
            user.Address = dto.Address ?? user.Address;
            user.BirthDate = dto.BirthDate ?? user.BirthDate;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return MapToProfile(user);
        }

        public async Task<List<UserProfileDto>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.FullName)
                .Select(u => MapToProfile(u))
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // ✅ Approve Trainer (Admin use)
        public async Task<bool> ApproveUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.IsApproved)
                return false;

            user.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SeedAdminAsync()
        {
            if (!await _context.Users.AnyAsync(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    FullName = "System Admin",
                    Email = "admin@gym.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsApproved = true
                };
                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }
        }

        private static UserProfileDto MapToProfile(User u) => new()
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address,
            BirthDate = u.BirthDate,
            IsApproved = u.IsApproved
        };
    }
}
