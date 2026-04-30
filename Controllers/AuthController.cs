using Dtos;
using Interfaces;
using Mappers;
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
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("jwt-key")]
        [ProducesResponseType(typeof(JwtDto), StatusCodes.Status200OK)]
        public IActionResult GetJwtKey()
        {
            string jwtKey = System.IO.File.ReadAllText(_config["Jwt:PublicKeyPath"]!);
            return Ok(jwtKey.ToJwtDto());
        }
    }
}
