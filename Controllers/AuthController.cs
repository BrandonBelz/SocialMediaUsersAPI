using Dtos;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(
            IUserRepository userRepository,
            TokenService service,
            IConfiguration config
        )
            : base(userRepository)
        {
            _tokenService = service;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            User? user = await _userRepo.GetUserAsync(dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
            {
                return Unauthorized();
            }

            string token = _tokenService.GenerateToken(user);
            Response.Cookies.Append(
                "jwt",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                }
            );
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }

        [Authorize]
        [HttpGet("jwt-key")]
        public IActionResult GetJwtKey()
        {
            return Ok(
                new { publicKey = System.IO.File.ReadAllText(_config["Jwt:PublicKeyPath"]!) }
            );
        }
    }
}
