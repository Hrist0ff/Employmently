using employmently_be.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("/User")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserContext _userContext;

        public UsersController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userContext.Users;
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _userContext.Users.FirstOrDefault(x => x.Id == id);
            return Ok(user);
        }
    }
}
