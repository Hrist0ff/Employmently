using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.Data.Models.DTOs;
using employmently_be.Data.Models.ViewModels;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace employmently_be.Controllers
{
    [Route("/User")]
    [ApiController]

    
    public class UsersController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;


        public UsersController(dbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult GetUsers()
        {
            var users = _dbContext.Users;
            return Ok(users);
           
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUser(string id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);

            if(user == null)
            {
                ModelState.AddModelError("Error", "There is no user with this id.");
                return BadRequest(ModelState);
            }

            var result = new GetUserByIdDto()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                UniqueIdentifierCompany = user.UniqueIdentifierCompany,
                Description = user.Description,
                ProfilePicture = user.ProfilePicture,
            };

            
            return Ok(result);
        }

        [HttpPost("Applicateforjob/{listingid}")]
        [Authorize(Roles ="Candidate")]
        public async Task<IActionResult> PostApplication([FromRoute]int listingid)
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                ModelState.AddModelError("Error", "User does not exists.");
                return BadRequest(ModelState);
            }

            var listing = _dbContext.Listings.Where(x => x.Id == listingid);

            if (listing == null)
            {
                ModelState.AddModelError("Error", "Listing does not exists.");
                return BadRequest(ModelState);
            }

            var listingApplication = _dbContext.ListingApplications.Where(x => x.Id == listingid && x.userId == user.Id);

            if(!listingApplication.Any())
            {
                var listingToAdd = new ListingApplications()
                {
                    listingId = listingid,
                    userId = user.Id,
                    applicationTime = DateTime.Now
                };
                _dbContext.ListingApplications.Add(listingToAdd);
                _dbContext.SaveChanges();

                return Ok("Your application has been sent to the company.");
            }


            ModelState.AddModelError("Error", "Listing application already exists.");
            return BadRequest(ModelState);
        }

        [HttpGet("getMyApplications")]
        [Authorize(Roles ="Candidate")]
        public async Task<IActionResult> getMyApplications()
        {
            var user = await _userManager.GetUserAsync(User);

            var applications = _dbContext.ListingApplications
                   .Join(_dbContext.Listings,
                         la => la.listingId,
                         l => l.Id,
                         (la, l) => new { ListingApplication = la, Listing = l })
                   .Where(x => x.ListingApplication.userId == user.Id)
                   .Select(x => new MyApplicationsViewModel
                   {
                       ListingName = x.Listing.Name,
                       ListingCompany = x.Listing.Author.Company.Name,
                       listingStatus = x.ListingApplication.status.ToString(),
                       applicationTime = x.ListingApplication.applicationTime
                   })
                   .ToList();

            if (applications == null)
            {
                return Ok("You don't have applications at the moment.");
            }
            return Ok(applications);
        }

    }

   
}
