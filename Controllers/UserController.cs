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
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] JsonObject requestJson
        )
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            User? user = await _userRepo.UpdateAsync(id, requestJson);

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
    }
}
