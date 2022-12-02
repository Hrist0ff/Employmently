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
        private readonly UserContext _userContext;
        private readonly UserManager<User> _userManager;

        public RegistrationController(UserContext userContext, UserManager<User> userManager)
        {
            _userContext = userContext;
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
            userToAdd.UserName = rdt.username;

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
            userToAdd.UserName = rdt.username;

            var result = await _userManager.CreateAsync(userToAdd, rdt.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var UserFromDb = await _userManager.FindByNameAsync(userToAdd.UserName);
            await _userManager.AddToRoleAsync(UserFromDb, "Company");
            return Ok();
        }

    }
}
