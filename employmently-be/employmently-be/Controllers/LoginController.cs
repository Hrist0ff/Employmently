using employmently_be.Data.Models;
using employmently_be.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("/Login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;

        public LoginController( UserManager<User> userManager)
        {
            _userManager = userManager;

        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto input)
        {   
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email Error", "Error in email check");
                return BadRequest(ModelState);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, input.Password);
            
            if (!passwordValid)
            {
                ModelState.AddModelError("Password Error", "Error in password");
                return BadRequest(ModelState);
            }
            return Ok();

        }
    }
}
