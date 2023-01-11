using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
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
        [Authorize]
        public IActionResult GetUser(string id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            var result = new GetUserByIdDto()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                UniqueIdentifierCompany = user.UniqueIdentifierCompany,
                Description = user.Description,
                ProfilePicture = user.ProfilePicture,
            };

            
            return Ok(result);
        }
    }

   
}
