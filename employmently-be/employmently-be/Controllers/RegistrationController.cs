using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("/Register")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public RegistrationController(dbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto rdt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userToAdd = new User();
            userToAdd.Email = rdt.Email;
            userToAdd.UserName = rdt.Username;

            var result = await _userManager.CreateAsync(userToAdd, rdt.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var UserFromDb = await _userManager.FindByNameAsync(userToAdd.UserName);
            await _userManager.AddToRoleAsync(UserFromDb, "Candidate");
            return Ok();
        }

        [HttpPost]
        [Route("Company")]
        public async Task<IActionResult> RegisterCompany(RegisterDto rdt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userToAdd = new User();
            userToAdd.Email = rdt.Email;
            userToAdd.UserName = rdt.Username;

            var result = await _userManager.CreateAsync(userToAdd, rdt.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var UserFromDb = await _userManager.FindByNameAsync(userToAdd.UserName);
            await _userManager.AddToRoleAsync(UserFromDb, "Company");
            // To add EIK as user's property and make another DTO here with the EIK
            return Ok();
        }

    }
}
