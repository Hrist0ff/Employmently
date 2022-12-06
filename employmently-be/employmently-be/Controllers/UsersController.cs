using employmently_be.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("/User")]
    [ApiController]

    
    public class UsersController : Controller
    {
        private readonly dbContext _dbContext;

        public UsersController(dbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetUsers()
        {
            var users = _dbContext.Users;
            return Ok(users);
           
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            return Ok(user);
        }
    }

   
}
