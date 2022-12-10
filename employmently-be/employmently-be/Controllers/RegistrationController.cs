using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;

namespace employmently_be.Controllers
{
    [Route("/Register")]
    [ApiController]
    public class RegistrationController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;
        private readonly IOptions<AuthMessageSenderOptions> optionsAccessor;

        public RegistrationController(dbContext dbContext, UserManager<User> userManager,IConfiguration config, ILogger<EmailSender> logger,
            IOptions<AuthMessageSenderOptions> options)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            optionsAccessor = options;

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
            userToAdd.EmailConfirmed = false;
            

            var result = await _userManager.CreateAsync(userToAdd, rdt.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userToAdd);
            var email_body = "Employmently <br></br>Confirm your email here <a href=\"#URL#\"> Link </a>";

            var confirmationLink = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail","Registration", new {code = code, userId = userToAdd.Id});
            var body = email_body.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmationLink));
            Console.WriteLine(body);
            EmailSender emailHelper = new EmailSender(optionsAccessor,_logger, _config);
            await emailHelper.SendEmailAsync(userToAdd.Email,"Confirm your email - Employmently",body);

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

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string code, string userId) {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            
            return result.Succeeded ? Ok() : BadRequest();

        }


    }
}
