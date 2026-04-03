using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Dtos;
using Mappers;

namespace Controllers
{

    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public UserController(ApplicationDBContext context) { _context = context; }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<UserPublicDto> users =
                _context.Users.Select(u => u.ToPublicDto()).ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            User? user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToPublicDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequestDto requestDto)
        {
            User newUser = requestDto.ToUserFromCreate();
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id },
                                   newUser.ToPrivateDto());
        }
    }
}
