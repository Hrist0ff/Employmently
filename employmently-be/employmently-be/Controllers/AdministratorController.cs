using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.Data.Models.ViewModels;
using employmently_be.DbContexts;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace employmently_be.Controllers
{
    [Route("/Admin")]
    [ApiController]


    public class AdministratorController : Controller
    {
        private readonly dbContext _dbContext;

        public AdministratorController(dbContext dbContext)
        {
            _dbContext = dbContext;
        }



        ///////////////////////////////////
        ///                             //
        ///   Accept or Reject Listings //
        ///                             //
        //////////////////////////////////
        [HttpPost]
        [Route("acceptListing/{listingId}")]
        [Authorize(Roles="Administrator")]
        public ActionResult AcceptList([FromRoute]int listingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (listingId == null)
            {
                return NotFound("Listing not found.");
            }

            var listing = _dbContext.Listings.Where(l => l.Id == listingId).FirstOrDefault();

            if (listing == null)
            {
                return NotFound("Listing not found.");
            }


            if (listing.Status == ListingStatus.Accepted)
            {
                return BadRequest("Listing already accepted.");
            }
            listing.Status = ListingStatus.Accepted;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("rejectListing/{listingId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> RejectListing([FromRoute] int listingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (listingId == null)
            {
                return NotFound("Listing not found.");
            }

            var listing = _dbContext.Listings.Where(l => l.Id == listingId).FirstOrDefault();

            if (listing == null)
            {
                return NotFound("Listing not found.");
            }


            if (listing.Status == ListingStatus.Rejected)
            {
                return BadRequest("Listing already rejected.");
            }
            listing.Status = ListingStatus.Rejected;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("getListings")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetListings()
        {

            //var listings = _dbContext.Listings.First();
            var listings = _dbContext.Listings.Where(l => l.Status == ListingStatus.Pending)
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
                           });
            if (!listings.Any())
            {
                return Ok();
            }
            return Ok(listings);
        }
    }
}