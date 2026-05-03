using WebApplication17.DTOs;
using WebApplication17.Models;

namespace WebApplication17.Services
{
    
        public interface IAuthService
        {
            string GenerateToken(User user);

        Task<bool> RegisterAsync(RegisterRequestDTO request);
        
    
}
}
