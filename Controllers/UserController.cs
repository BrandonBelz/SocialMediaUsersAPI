using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dtos;
using Mappers;
using Interfaces;

namespace Controllers
{

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IUserRepository _userRepo;

        public UserController(ApplicationDBContext context,
                              IUserRepository userRepository)
        {
            _userRepo = userRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<User> users = await _userRepo.GetUsersAsync();

            return Ok(users.Select(u => u.ToPublicDto()));
        }

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

        [HttpPost]
        public async Task<IActionResult>
        Create([FromBody] CreateUserRequestDto requestDto)
        {
            User newUser =
                await _userRepo.CreateAsync(requestDto.ToUserFromCreate());
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id },
                                   newUser.ToPrivateDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult>
        Update([FromRoute] int id, [FromBody] UpdateUserRequestDto requestDto)
        {
            User? user = await _userRepo.UpdateAsync(id, requestDto);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToPrivateDto());
        }

        [HttpDelete]
        [Route("{id}")]
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
