using System.Security.Claims;
using Data;
using Dtos;
using Interfaces;
using Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;
        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public UserController(ApplicationDBContext context, IUserRepository userRepository)
        {
            _userRepo = userRepository;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? query)
        {
            List<User> users = await _userRepo.GetUsersAsync(query);

            return Ok(users.Select(u => u.ToMinimizedDto()));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            User? user = await _userRepo.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToPublicDto());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto requestDto)
        {
            User newUser = await _userRepo.CreateAsync(requestDto.ToUserFromCreate());
            return CreatedAtAction(
                nameof(GetById),
                new { id = newUser.Id },
                newUser.ToPrivateDto()
            );
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateUserRequestDto requestDto
        )
        {
            User? user = await _userRepo.UpdateAsync(id, requestDto);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToPrivateDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            User? toDelete = await _userRepo.DeleteAsync(id);

            if (toDelete == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
