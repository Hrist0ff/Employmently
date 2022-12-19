using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("/Profile")]
    [ApiController]


    public class ProfileController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ProfileController(dbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPut("changeDescription/{id}")]
        [Authorize(Roles = "Company,Candidate")]
        public async Task<IActionResult> ChangeDesc(string id, [FromBody]string description)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }


            if(requestedUser.Id == currentUser.Id)
            {
                currentUser.Description = description;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();

        }
    }
}