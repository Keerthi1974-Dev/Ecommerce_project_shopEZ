using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication17.DTOs;
using WebApplication17.Models;
using WebApplication17.Services;

namespace WebApplication17.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // REGISTER 
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and Password are required");

            var result = await _authService.RegisterAsync(request);

            if (!result)
                return BadRequest("This Record already exists");

            return Ok("The user registered successfully!!");
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid request");

            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and Password are required");

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password.Trim(),user.Password))
                    return Unauthorized("Invalid email or password!");

                var token = _authService.GenerateToken(user);

                return Ok(new
                {
                    token,
                    user = new
                    {
                        user.UserId,
                        user.Name,
                        user.Email,
                        user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}