using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dtos;
using Mappers;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public UserController(ApplicationDBContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<UserPublicDto> users =
                await _context.Users.Select(u => u.ToPublicDto()).ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            User? user = await _context.Users.FindAsync(id);

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
            User newUser = requestDto.ToUserFromCreate();
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id },
                                   newUser.ToPrivateDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult>
        Update([FromRoute] int id, [FromBody] UpdateUserRequestDto requestDto)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = requestDto.Email;
            user.Username = requestDto.Username;
            user.Biography = requestDto.Biography;

            await _context.SaveChangesAsync();
            return Ok(user.ToPrivateDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            User? toDelete =
                await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (toDelete == null)
            {
                return NotFound();
            }

            _context.Users.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
