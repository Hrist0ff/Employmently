using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace employmently_be.Controllers
{
    [Route("/Forgotpassword")]
    [ApiController]
    public class ForgotPasswordController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;
        private readonly IOptions<AuthMessageSenderOptions> optionsAccessor;

        public ForgotPasswordController(dbContext dbContext, UserManager<User> userManager, IConfiguration config, ILogger<EmailSender> logger,
            IOptions<AuthMessageSenderOptions> options)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            optionsAccessor = options;

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto fpc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(fpc.Email);

            if (user == null)
            {
                ModelState.AddModelError("Error", "There is no such a user.");
                return BadRequest();
            }

            var forgotPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var forgotPassword = new ForgotPassword
            {
                UserId = user.Id,
                Token = forgotPasswordToken
            };

            var encodedToken = System.Web.HttpUtility.UrlEncode(forgotPasswordToken);


            _dbContext.ForgotPasswords.Add(forgotPassword);
            await _dbContext.SaveChangesAsync();

            var email_body = "Employmently <br></br> You can change your password by clicking <a href=\"#URL#\"> here </a>";

            var confirmationLink = "http://localhost:3000/Changepassword/id=" + user.Id + "/token=" + encodedToken;
            var body = email_body.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmationLink));
            body.Replace("#username#", user.UserName);
            EmailSender emailHelper = new EmailSender(optionsAccessor, _logger, _config);
            await emailHelper.SendEmailAsync(user.Email, "Change your password - Employmently", body);

            return Ok();
        }


        [HttpPost]
        [Route("Changepassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto cpd)
        {
            var user = await _userManager.FindByIdAsync(cpd.userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ResetPasswordAsync(user, cpd.token, cpd.NewPassword);

            if (result.Succeeded)
            {
                var token = _dbContext.ForgotPasswords.Where(t => t.Token == cpd.token).FirstOrDefault();
                if(token != null)
                {
                    _dbContext.ForgotPasswords.Remove(token);
                    await _dbContext.SaveChangesAsync();
                }
                return Ok();
            }

            ModelState.AddModelError("Error", "Couldn't change password");
            return BadRequest(ModelState);

        }
    }
}
