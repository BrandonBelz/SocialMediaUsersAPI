using System.Security.Claims;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IUserRepository _userRepo;
        protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public BaseController(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
    }
}
