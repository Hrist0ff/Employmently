using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.ServiceModel;
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
            Console.WriteLine(rdt.ConfirmPassword);
            Console.WriteLine(rdt.Password);
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
                return BadRequest(result.Errors.First().Description);
            }

            // If Register is successful we send confirmation email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userToAdd);
            var email_body = "Employmently <br></br>Confirm your email <a href=\"#URL#\"> here </a>";

            var confirmationLink = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail","Registration", new {code = code, userId = userToAdd.Id});
            var body = email_body.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmationLink));
            Console.WriteLine(body);
            EmailSender emailHelper = new EmailSender(optionsAccessor,_logger, _config);
            await emailHelper.SendEmailAsync(userToAdd.Email,"Confirm your email - Employmently",body);


            // Add User a role
            var UserFromDb = await _userManager.FindByNameAsync(userToAdd.UserName);
            await _userManager.AddToRoleAsync(UserFromDb, "Candidate");
            return Ok();
        }

        [HttpPost]
        [Route("Company")]
        public async Task<IActionResult> RegisterCompany(RegisterCompanyDto rdt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Create user to add model
            var userToAdd = new User();
            userToAdd.Email = rdt.Email;
            userToAdd.UserName = rdt.Username;
            userToAdd.UniqueIdentifierCompany = rdt.CompanyId;

            // Create a user if we can
            var result = await _userManager.CreateAsync(userToAdd, rdt.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.First().Description);
            }

            // If Register is successful we send confirmation email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userToAdd);
            var email_body = "Employmently <br></br>Confirm your email <a href=\"#URL#\"> here </a>";

            var confirmationLink = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Registration", new { code = code, userId = userToAdd.Id });
            var body = email_body.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmationLink));
            Console.WriteLine(body);
            EmailSender emailHelper = new EmailSender(optionsAccessor, _logger, _config);
            await emailHelper.SendEmailAsync(userToAdd.Email, "Confirm your email - Employmently", body);


            // Check if UIC exists in a company, if so we add the user to the company
            // if not we make the new company and add the user in it
            var UIC = userToAdd.UniqueIdentifierCompany;
            var flag = 0;
            var userFromDb = await _userManager.FindByEmailAsync(userToAdd.Email);

            // Here i use external web service which is provided by European Commission for getting the name of a company by given Vat number
            var vat = new ServiceReference1.checkVatPortTypeClient();
            bool blnValid;
            string companyName;
            string strAddress;
            string strCountryCode = "BG";
            string strVatNumber = UIC;
            var t = vat.checkVat(ref strCountryCode, ref strVatNumber, out blnValid, out companyName, out strAddress);

            foreach (Company company in _dbContext.Companies)
            {
                if(company.UniqueIdentifier == UIC)
                {
                    company.Users.Add(userToAdd);
                    flag = 1;
                }
            }

            if(flag == 0)
            {
                if (blnValid == true)
                {
                    var newCompany = new Company()
                    {
                        UniqueIdentifier = UIC,
                        Name = companyName
                    };
                    newCompany.Users.Add(userFromDb);
                    _dbContext.Companies.Add(newCompany);
                }
                else
                {

                }
            }
            await _dbContext.SaveChangesAsync();

            // Add User a role
            var UserFromDb = await _userManager.FindByNameAsync(userToAdd.UserName);
            await _userManager.AddToRoleAsync(UserFromDb, "Company");
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

