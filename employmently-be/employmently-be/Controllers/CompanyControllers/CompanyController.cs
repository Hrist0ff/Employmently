using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using employmently_be.Data.Entities;
using employmently_be.Data.Models.ViewModels;
using employmently_be.DbContexts;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace employmently_be.Controllers.Company
{
    [Route("/Company")]
    [ApiController]

    public class CompanyController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;
        private readonly IOptions<AuthMessageSenderOptions> optionsAccessor;
        public CompanyController(dbContext dbContext, UserManager<User> userManager, IConfiguration config, ILogger<EmailSender> logger,
            IOptions<AuthMessageSenderOptions> options)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
            _logger = logger;
            optionsAccessor = options;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> GetCompany(string id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            var currentUser = await _userManager.GetUserAsync(User);


            if (currentUser == null || user == null)
            {
                return NotFound();
            }

            if(currentUser.Id != user.Id)
            {
                return BadRequest();
            }

            var company = _dbContext.Companies.FirstOrDefault(x => x.UniqueIdentifier == user.UniqueIdentifierCompany);


            var technologies = _dbContext.Listings
                         .Where(l => l.Status == ListingStatus.Accepted && l.Author.UniqueIdentifierCompany == company.UniqueIdentifier)
                         .Select(l => new ListingViewModel()
                         {
                             Id = l.Id,
                             Name = l.Name,
                             Description = l.Description,
                             CreatedDate = l.CreatedDate,
                    // include other properties (Authors and Categories) of the listing here
                    AuthorId = l.Author.Id,
                             AuthorName = l.Author.Company.Name,
                             CategoryNames = l.Categories.Select(c => c.Name),
                             Location = l.Location,
                             Arrangement = l.Arrangement,
                             Salary = l.Salary,
                             AuthorPic = l.Author.Company.ProfilePicture,
                         }).SelectMany(c => c.CategoryNames).Distinct();



            if (company == null)
            {
                return NotFound();
            }

            var result = new MyCompanyViewModel()
            {
                CompanyId = company.Id,
                Name = company.Name,
                UniqueIdentifier = company.UniqueIdentifier,
                Description = company.Description,
                YearCreated = company.YearCreated,
                ProfilePicture = company.ProfilePicture,
                Employees = company.Employees,
                PhoneNumber = company.PhoneNumber,
                Technologies = technologies

            };

            return Ok(result);
        }

        [HttpDelete("deleteListing/{id}")]
        [Authorize(Roles = "Company" )]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                ModelState.AddModelError("Error", "There's no such a user");
                return BadRequest(ModelState);
            }

            var listing = _dbContext.Listings.Where(l => l.Id == id).FirstOrDefault();

            var applications = _dbContext.ListingApplications.Where(a => a.listingId == id);


            if (listing == null)
            {
                ModelState.AddModelError("Error", "There's no such listing");
                return BadRequest(ModelState);
            }

            if(listing.Author.Id != user.Id)
            {
                ModelState.AddModelError("Error", "You are trying to delete a listing, which is not yours");
                return BadRequest(ModelState);
            }

            _dbContext.Listings.Remove(listing);
            if(applications != null)
            {
                _dbContext.ListingApplications.RemoveRange(applications);
            }
            await _dbContext.SaveChangesAsync();
            return Ok();


        }

        [HttpPut("changeDescription/{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> ChangeDesc([FromRoute]string id, [FromBody] string description)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                ModelState.AddModelError("Error", "There's no such a user");
                return BadRequest(ModelState);
            }
            if(currentUser.Id != requestedUser.Id)
            {
                ModelState.AddModelError("Error", "User's not the same as requesting user");
                return BadRequest(ModelState);
            }

            if(description == null)
            {
                ModelState.AddModelError("Error", "Description can't be empty.");
                return BadRequest(ModelState);
            }

            
            var company = _dbContext.Companies.FirstOrDefault(x => x.UniqueIdentifier == currentUser.UniqueIdentifierCompany);

            company.Description = description;
            await _dbContext.SaveChangesAsync();
            return Ok();
            
        }

        [HttpPut("changePhoneNumber/{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> changePhoneNumber(string id, [FromBody] string number)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }

            if (currentUser.Id != requestedUser.Id)
            {
                ModelState.AddModelError("Error", "User's not the same as requesting user");
                return BadRequest(ModelState);
            }

            var company = _dbContext.Companies.FirstOrDefault(x => x.UniqueIdentifier == currentUser.UniqueIdentifierCompany);

            company.PhoneNumber = number;
            await _dbContext.SaveChangesAsync();
            return Ok();
            

        }

        [HttpPut("changeYearOfCreation/{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> ChangeYear( [FromRoute] string id , [FromBody] string yearToChange)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);


            if (currentUser == null || requestedUser == null)
            {
                ModelState.AddModelError("Error", "There's no such a user");
                return BadRequest(ModelState);
            }

            var company = _dbContext.Companies.FirstOrDefault(x => x.UniqueIdentifier == currentUser.UniqueIdentifierCompany);

            if (int.TryParse(yearToChange, out int year) && (year >= 1950 && year <= 2023))
            {
                company.YearCreated = yearToChange;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            ModelState.AddModelError("Error", "Provided year is not valid");
            return BadRequest(ModelState);
        }

        [HttpPut("changeEmployees/{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> ChangeEmployees([FromRoute] string id, [FromBody] string employeesQuantity)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);


            if (currentUser == null || requestedUser == null)
            {
                ModelState.AddModelError("Error", "There's no such a user");
                return BadRequest(ModelState);
            }

            var company = _dbContext.Companies.FirstOrDefault(x => x.UniqueIdentifier == currentUser.UniqueIdentifierCompany);
            string[] allowedOptions = { "1-5", "5-15", "15-30" , "30-50", "50-100", "100-300", "300+"};

            if (!allowedOptions.Contains(employeesQuantity))
            {
                ModelState.AddModelError("Error", "Provided quantity is not valid");
                return BadRequest(ModelState);
            }

            company.Employees = employeesQuantity;
            await _dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPost]
        [Route("uploadPic/{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> PostImage([FromRoute] string id, IFormFile image)
        {
            if (!image.ContentType.Equals("image/png"))
            {
                return BadRequest("Only PNG files are allowed");
            }

            // Connect to Azure Storage
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=employmentlystorage;AccountKey=tQbLLmAfixQIMKPgvlmporcKOUaJ4phqihnrdOlm0450u9bm5iQ/HZ7/+PQ3QKc4wI6xfdIHKxDt+ASthlVoeQ==;EndpointSuffix=core.windows.net";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("fileupload");
            await containerClient.CreateIfNotExistsAsync();


            var requestedUser = await _userManager.FindByIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }



            if (requestedUser.Id == currentUser.Id)
            {
                var company = _dbContext.Companies.FirstOrDefault(x => x.UniqueIdentifier == currentUser.UniqueIdentifierCompany);
                string imageName = company.UniqueIdentifier + "_profilepic" + Path.GetExtension(image.FileName);
                BlobClient blobClient = containerClient.GetBlobClient(imageName);
                using (var stream = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
                }
                company.ProfilePicture = blobClient.Uri.AbsoluteUri;
                await _dbContext.SaveChangesAsync();
                return Ok(blobClient.Uri.AbsoluteUri);
            }
            return BadRequest();
        }

        [HttpPost("acceptApplication/{id}")]
        [Authorize(Roles ="Company")]
        public async Task<IActionResult> acceptApplication([FromRoute]int id,[FromBody]string DateForInterview)
        {
            DateTime date = DateTime.Parse(DateForInterview, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError("Error", "User does not exists.");
                return BadRequest(ModelState);
            }

            var listingApplication = _dbContext.ListingApplications.Where(x => x.Id == id).FirstOrDefault();

            if (listingApplication == null)
            {
                ModelState.AddModelError("Error","Listing does not exists.");
                return BadRequest(ModelState);
            }

            var listing = _dbContext.Listings
                .Include(x => x.Author)
                .Include(x => x.Author.Company)
                .Where(x => x.Id == listingApplication.listingId).FirstOrDefault();

            if(listing == null)
            {
                return BadRequest();
            }

            if(listing.Author.UniqueIdentifierCompany != user.UniqueIdentifierCompany)
            {
                ModelState.AddModelError("Error", "You are trying to accept application to other company.");
                return BadRequest(ModelState);

            }

            if(listingApplication.status != ListingStatus.Pending)
            {
                ModelState.AddModelError("Error", "You are trying to accept application which already has been accepted/rejected.");
                return BadRequest(ModelState);
            }


            string dateString = date.ToString("dd/MM/yyyy HH:mm");


            var userEmail = await _dbContext.Users.Where(x => x.Id == listingApplication.userId).FirstOrDefaultAsync();
            var subject = "Your application for " + listing.Name + " " + "in " + listing.Author.Company.Name + " has been accepted";
            var email_body = "Employmently <br></br>" + subject + "<br></br>Suggested interview date: " + dateString
                + "<br></br>Company contact details:<br></br>Phone number: " + listing.Author.Company.PhoneNumber;



            EmailSender emailHelper = new EmailSender(optionsAccessor, _logger, _config);


            await emailHelper.SendEmailAsync(userEmail.Email, subject, email_body);


            listingApplication.status = ListingStatus.Accepted;
            listingApplication.suggestedInterviewDate = date;
            _dbContext.SaveChanges();
            return Ok("Listing application has been accepted.");

        }

        [HttpPost("rejectApplication/{id}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> rejectApplication([FromRoute] int id, [FromBody] string rejectionPurpose)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                ModelState.AddModelError("Error", "User does not exists.");
                return BadRequest(ModelState);
            }

            var listingApplication = _dbContext.ListingApplications.Where(x => x.Id == id).FirstOrDefault();

            if (listingApplication == null)
            {
                ModelState.AddModelError("Error", "Listing does not exists.");
                return BadRequest(ModelState);
            }

            var listing = _dbContext.Listings
                .Include(x => x.Author)
                .Include(x => x.Author.Company)
                .Where(x => x.Id == listingApplication.listingId).FirstOrDefault();

            if (listing == null)
            {
                return BadRequest();
            }

            if (listing.Author.UniqueIdentifierCompany != user.UniqueIdentifierCompany)
            {
                ModelState.AddModelError("Error", "You are trying to reject application to other company.");
                return BadRequest(ModelState);

            }

            if (listingApplication.status != ListingStatus.Pending)
            {
                ModelState.AddModelError("Error", "You are trying to reject application which already has been accepted/rejected.");
                return BadRequest(ModelState);
            }


            var userEmail = await _dbContext.Users.Where(x => x.Id == listingApplication.userId).FirstOrDefaultAsync();
            var subject = "Your application for " + listing.Name + " in " + listing.Author.Company.Name + " has been rejected";
            var email_body = "Employmently <br></br>" + subject + "<br></br> The reason is: " + rejectionPurpose;


            EmailSender emailHelper = new EmailSender(optionsAccessor, _logger, _config);

            await emailHelper.SendEmailAsync(userEmail.Email, subject, email_body);


            listingApplication.status = ListingStatus.Rejected;
            listingApplication.rejectionPurpose = rejectionPurpose;
            _dbContext.SaveChanges();
            return Ok("Listing application has been rejected.");

        }

        [HttpGet("getApplicationsToCompany")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> getApplicationsToCompany()
        {
            var user = await _userManager.GetUserAsync(User);

            var listingsOfCompany = _dbContext.Listings
                .Where(x => x.Author.UniqueIdentifierCompany == user.UniqueIdentifierCompany && x.Status == ListingStatus.Accepted)
                .Select(x => x.Id);

            var applications = _dbContext.ListingApplications
             .Join(_dbContext.Users,
                 app => app.userId,
                 usr => usr.Id,
                 (app, usr) => new { Application = app, User = usr })
              .Join(_dbContext.Listings,
                 app => app.Application.listingId,
                 lst => lst.Id,
                 (app, lst) => new { Application = app.Application, User = app.User, Listing = lst })
             .Where(x => listingsOfCompany.Contains(x.Application.listingId) && x.Application.status == ListingStatus.Pending)
             .Select(x => new
             {
                 id = x.Application.Id,
                 cv = x.Application.CV,
                 motivationalLetter = x.Application.motivationalLetter,
                 listingId = x.Application.listingId,
                 rejectionPurpose = x.Application.rejectionPurpose,
                 status = x.Application.status,
                 suggestedInterviewDate = x.Application.suggestedInterviewDate,
                 userId = x.User.Id,
                 userName = x.User.UserName,
                 userDescription = x.User.Description,
                 userPic = x.User.ProfilePicture,
                 applicationTime = x.Application.applicationTime,
                 listingName = x.Listing.Name,
                 listingCategories = x.Listing.Categories.Select(c => c.Name),
                 listingArrangement = x.Listing.Arrangement,
             });

            if (applications == null)
            {
                return Ok("You don't have applications at the moment.");
            }
            return Ok(applications);




        }

    }
}
