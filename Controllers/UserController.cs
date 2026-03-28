using Data;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers {

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase {
    private readonly ApplicationDBContext _context;

    public UserController(ApplicationDBContext context) { _context = context; }

    [HttpGet]
    public IActionResult GetAll() {
        List<User> users = _context.Users.ToList();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id) {
        User? user = _context.Users.Find(id);

        if (user == null) {
            return NotFound();
        }

        return Ok(user);
    }
}
}
