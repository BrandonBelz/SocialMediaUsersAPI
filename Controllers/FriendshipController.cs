using Dtos;
using Interfaces;
using Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [Route("api/user/{id}/friends")]
    [ApiController]
    public class FriendshipController : BaseController
    {
        public FriendshipController(IUserRepository repo)
            : base(repo) { }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<UserMinimizedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFriends([FromRoute] int id)
        {
            List<User>? friends = await _userRepo.GetFriendsAsync(id);

            if (friends == null)
            {
                return NotFound();
            }

            return Ok(friends.Select(u => u.ToMinimizedDto()));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddFriend([FromRoute] int id, [FromBody] int otherId)
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            Friendship? friendship = await _userRepo.GetFriendshipAsync(id, otherId);

            if (friendship != null)
            {
                return Conflict("Friendship already exists.");
            }

            friendship = await _userRepo.AcceptRequest(id, otherId);

            if (friendship == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize]
        [HttpDelete("{friendId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveFriend([FromRoute] int id, [FromRoute] int friendId)
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            Friendship? friendship = await _userRepo.RemoveFriend(id, friendId);

            if (friendship == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("requests/sent")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<UserMinimizedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSentRequests([FromRoute] int id)
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            List<User>? requestedFriends = await _userRepo.GetSentRequestsAsync(id);

            if (requestedFriends == null)
            {
                return NotFound();
            }

            return Ok(requestedFriends.Select(u => u.ToMinimizedDto()));
        }

        [Authorize]
        [HttpGet("requests/received")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<UserMinimizedDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReceivedRequests([FromRoute] int id)
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            List<User>? requestingUsers = await _userRepo.GetReceivedRequestsAsync(id);

            if (requestingUsers == null)
            {
                return NotFound();
            }

            return Ok(requestingUsers.Select(u => u.ToMinimizedDto()));
        }

        [Authorize]
        [HttpPost("requests")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> SendFriendRequest(
            [FromRoute] int id,
            [FromBody] int otherId
        )
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            Friendship? friendship = await _userRepo.GetFriendshipAsync(id, otherId);

            if (friendship != null)
            {
                return Conflict("A friendship exists between these two users.");
            }

            FriendRequest? friendRequest = await _userRepo.GetFriendRequestAsync(id, otherId);

            if (friendRequest != null)
            {
                return Conflict("A friend request is already active between these users.");
            }

            friendRequest = await _userRepo.SendRequest(id, otherId);

            if (friendRequest == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize]
        [HttpDelete("requests/{friendId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteFriendRequest(
            [FromRoute] int id,
            [FromRoute] int friendId
        )
        {
            if (!User.IsInRole("Service") && CurrentUserId != id)
            {
                return Unauthorized();
            }

            FriendRequest? friendRequest = await _userRepo.DenyRequest(id, friendId);

            if (friendRequest == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
