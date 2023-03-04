using employmently_be.Data.Entities;
using employmently_be.Data.Models.ViewModels;
using employmently_be.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace employmently_be.Controllers
{
    [Route("Filter")]
    [ApiController]
    public class FilterController : Controller
    {
        private readonly dbContext _dbContext;

        public FilterController(dbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<Listing>> GetListingsByFilters([FromQuery] int minSalary, [FromQuery] int maxSalary, [FromQuery] bool salaryStated,
              [FromQuery] List<string> locations,[FromQuery] List<string> categories,[FromQuery] string? arrangement)
        {
            var listings = _dbContext.Listings.Where(l => l.Status == ListingStatus.Accepted)
                .Select(l => new ListingViewModel()
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                CreatedDate = l.CreatedDate,
                AuthorId = l.Author.Id,
                CompanyId = l.Author.Company.Id,
                AuthorName = l.Author.Company.Name,
                CategoryNames = l.Categories.Select(c => c.Name),
                Location = l.Location,
                Arrangement = l.Arrangement,
                Salary = l.Salary,
                AuthorPic = l.Author.Company.ProfilePicture,
            }); ;
        
            var location = locations.FirstOrDefault();

            if (minSalary > 0)
            {
                listings = listings.Where(l => l.Salary > minSalary);
            }
            
            if (maxSalary > 0)
            {
                listings = listings.Where(l => l.Salary <= maxSalary);
            }
            
            if (categories.Count > 0)
            {
                listings = listings.Where(l => l.CategoryNames.Any(c => categories.Contains(c)));
            }

            if (locations.Count > 0)
            {
                listings = listings.Where(l => l.Location == location);
            }

            if (salaryStated == true)
            {
                listings = listings.Where(l => l.Salary > 0);
            }

            if (arrangement != null)
            {
                listings = listings.Where(l=> l.Arrangement == arrangement);
            }

            return Ok(listings);
        }
    }
}
