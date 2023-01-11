using employmently_be.Data.Models.ViewModels;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace employmently_be.Controllers.IdentityControllers
{
    [Route("/Token")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public TokenController(UserManager<User> userManager, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;

        }

        [HttpGet]
        [Route("expiredToken/{refreshToken}")]
        [Authorize]
        public async Task<IActionResult> CheckExpiredToken([FromRoute] string refreshToken)
        {
            refreshToken = HttpUtility.UrlDecode(refreshToken);
            Console.WriteLine(refreshToken);
            var storedRefreshToken = await _refreshTokenRepository.GetAsync(refreshToken);

            if (storedRefreshToken == null)
            {
                ModelState.AddModelError("Error", "There is no refresh token.");
                return BadRequest(ModelState);
            }

            if (storedRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                await _refreshTokenRepository.DeleteAsync(storedRefreshToken);
                ModelState.AddModelError("Error", "Refresh token has expired.");
                return BadRequest(ModelState);
            }

            return Ok("Refresh token is not expired.");


        }

        [HttpPost]
        [Route("getNewAccessToken")]
        public async Task<ActionResult<AccessTokenViewModel>> getAccessToken([FromBody] string refreshToken)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                ModelState.AddModelError("Error", "Cannot find user.");
                return BadRequest(ModelState);
            }

            var storedRefreshToken = await _refreshTokenRepository.GetRefreshToken(currentUser.Id);
            Console.Write(storedRefreshToken);

            if (storedRefreshToken == null)
            {
                ModelState.AddModelError("Error", "There is no refresh token.");
                return BadRequest(ModelState);
            }

            if(storedRefreshToken.Token != refreshToken)
            {
                ModelState.AddModelError("Error", "Refresh token is not valid.");
                return BadRequest(ModelState);
            }

            if(storedRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                await _refreshTokenRepository.DeleteAsync(storedRefreshToken);
                ModelState.AddModelError("Error", "Refresh token has expired.");
                return BadRequest(ModelState);
            }

            var role = await _userManager.GetRolesAsync(currentUser);
            var token = GenerateJWTtoken(currentUser, role);
            return new AccessTokenViewModel()
            {
                accessToken = token
            };


            ModelState.AddModelError("Error", "Cannot provide token.");
            return BadRequest(ModelState);
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
              expires: DateTime.Now.AddSeconds(1),
              signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }



    }

}
