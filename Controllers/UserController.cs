using System.Text.Json.Nodes;
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
    public class UserController : BaseController
    {
        public UserController(IUserRepository userRepository)
            : base(userRepository) { }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<UserMinimizedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? query)
        {
            List<User> users = await _userRepo.GetUsersAsync(query);

            return Ok(users.Select(u => u.ToMinimizedDto()));
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserPublicDto), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(UserPrivateDto), StatusCodes.Status201Created)]
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
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserPrivateDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] UpdateUserRequestDto requestDto
        )
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            User? user = await _userRepo.UpdateAsync(id, requestDto);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToPrivateDto());
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            User? toDelete = await _userRepo.DeleteAsync(id);

            if (toDelete == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UserPrivateDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMe()
        {
            if (User.IsInRole("Service"))
            {
                return NotFound();
            }

            User? me = await _userRepo.GetUserAsync(CurrentUserId);

            if (me == null)
            {
                return NotFound();
            }

            return Ok(me.ToPrivateDto());
        }

        [Authorize]
        [HttpGet("{id}/profile")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProfile([FromRoute] int id)
        {
            if (User.IsInRole("Service") || CurrentUserId == id)
            {
                return Unauthorized();
            }

            User? user = await _userRepo.GetUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToProfileDto(CurrentUserId));
        }
    }
}
