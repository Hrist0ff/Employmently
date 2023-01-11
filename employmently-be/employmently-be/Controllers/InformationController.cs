using employmently_be.Data.Entities;
using employmently_be.Data.Models.ViewModels;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("/Info")]
    [ApiController]
    public class InformationController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public InformationController(dbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPost("getAcceptedListings")]
        [AllowAnonymous]
        public async Task<IActionResult> getAcceptedListings()
        {
            var listings = _dbContext.Listings.Where(l => l.Status == ListingStatus.Accepted)
               .Select(l => new ListingViewModel()
               {
                   Id = l.Id,
                   Name = l.Name,
                   Description = l.Description,
                   CreatedDate = l.CreatedDate,
                               // include other properties (Authors and Categories) of the listing here
                               AuthorId = l.Author.Id,
                   AuthorName = l.Author.Company.Name,
                   CategoryNames = l.Categories.Select(c => c.Name)
               });
            return Ok(listings);
        }

    }
}
