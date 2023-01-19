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
using employmently_be.Services;
using System.Security.Cryptography;
using employmently_be.Data.Entities;
using employmently_be.Data.Models.ViewModels;

namespace employmently_be.Controllers
{
    [Route("/Login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginController( UserManager<User> userManager, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<AuthenticationViewModel>> Login(LoginDto input)
        {   
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                ModelState.AddModelError("Error", "There's no such a user");
                return BadRequest(ModelState);
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, input.Password);

            if (!passwordValid)
            {
                ModelState.AddModelError("Error", "Error in password");
                return BadRequest(ModelState);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("Error", "Email is not confirmed! Please check your email and confirm it.");
                return BadRequest(ModelState);
            }
            var role = await _userManager.GetRolesAsync(user);
            var token = GenerateJWTtoken(user, role);
            var storedRefreshToken = await _refreshTokenRepository.GetRefreshToken(user.Id);

            if (storedRefreshToken != null)
            {
                if (storedRefreshToken.ExpirationDate < DateTime.UtcNow)
                {
                    await _refreshTokenRepository.DeleteAsync(storedRefreshToken);
                }
                else
                {
                    return new AuthenticationViewModel()
                    {
                        accessToken = token,
                        refreshToken = storedRefreshToken.Token
                    };
                }
            }
            var refreshToken = GenerateRefreshToken();

            await _refreshTokenRepository.CreateAsync(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddDays(1)
            });

            return new AuthenticationViewModel()
            {
                accessToken = token,
                refreshToken = refreshToken
            };

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
              expires: DateTime.Now.AddSeconds(1200),
              signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
            
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
