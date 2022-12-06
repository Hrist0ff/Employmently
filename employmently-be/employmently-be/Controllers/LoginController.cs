using employmently_be.Data.Models;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace employmently_be.Controllers
{
    [Route("/Login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public LoginController( UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto input)
        {   
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email Error", "There's no such a user");
                return BadRequest(ModelState);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, input.Password);

            
            if (!passwordValid)
            {
                ModelState.AddModelError("Password Error", "Error in password");
                return BadRequest(ModelState);
            }
            var role = await _userManager.GetRolesAsync(user);
            var token = GenerateJWTtoken(user, role);
            return Ok(token);

        }

        private string GenerateJWTtoken(User user, IList<string> role) 
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string roleS = string.Join("", role);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.Role, roleS)
                
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
