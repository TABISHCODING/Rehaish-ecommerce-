using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyntraClone.API.Data;
using MyntraClone.API.DTOs.Auth;
using MyntraClone.API.Models;
using MyntraClone.API.Services;
using Microsoft.AspNetCore.Authorization;    // ✅ Needed to use [AllowAnonymous]
using Microsoft.AspNetCore.Identity;         // ✅ Needed for secure password hashing

namespace MyntraClone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;  // ✅ Injected for secure password hashing

        public AuthController(ApplicationDbContext context, JwtService jwtService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        // ✅ POST: /api/auth/register
        // 🔓 [AllowAnonymous] allows this endpoint to be accessed without JWT token
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // ✅ Model validation: automatically checks [Required], [EmailAddress], etc.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return BadRequest("Email already in use.");

            // Create new user object
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = "Customer",
                Phone = dto.Phone, // Add this if present in RegisterDto
                Gender = dto.Gender, // Add this if present in RegisterDto
                DateOfBirth = dto.DateOfBirth // Add this if present in RegisterDto
                // PasswordHash will be set after creation
            };

            // ✅ Securely hash the password using ASP.NET Core Identity's PasswordHasher
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            // Add cart & wishlist - required related entities
            user.Cart = new Cart();
            user.Wishlist = new Wishlist();

            // Save user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate token with updated method that includes proper claims
            var token = _jwtService.GenerateToken(user);

            // Log token info for debugging
            Console.WriteLine($"Registration successful. User ID: {user.Id}, Role: {user.Role}");

            // Return authentication response with token
            return Ok(new AuthResponseDto
            {
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            });
        }

        // ✅ POST: /api/auth/login
        // 🔓 [AllowAnonymous] allows login without a token (as expected)
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // ✅ Model validation
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
            if (user == null) return Unauthorized("Invalid credentials.");

            // ✅ Verify hashed password using ASP.NET Core Identity
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials.");

            // Generate token with our updated method that includes proper claims
            var token = _jwtService.GenerateToken(user);

            // Log login attempt for debugging
            Console.WriteLine($"Login successful. User ID: {user.Id}, Role: {user.Role}");

            // Return authentication response
            return Ok(new AuthResponseDto
            {
                Token = token,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            });
        }

        // POST: /api/auth/change-password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get user ID from JWT claims
            var userIdStr = User.Identity?.Name;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized("User not found.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized("User not found.");

            // Verify current password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
                return BadRequest("Current password is incorrect.");

            // Hash and set new password
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully." });
        }
    }
}
