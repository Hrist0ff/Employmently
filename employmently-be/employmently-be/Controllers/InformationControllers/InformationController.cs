﻿using employmently_be.Data.Entities;
using employmently_be.Data.Models.ViewModels;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace employmently_be.Controllers
{
    [Route("/Information")]
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

        [HttpGet("getAcceptedListings")]
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
                   CompanyId = l.Author.Company.Id,
                   CategoryNames = l.Categories.Select(c => c.Name),
                   Location = l.Location,
                   Arrangement = l.Arrangement,
                   Salary = l.Salary,
                   AuthorPic = l.Author.Company.ProfilePicture,
               }); ;
            return Ok(listings);
        }


        [HttpGet("getCompanyListings/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getCompanyListings([FromRoute] int id)
        {
            var listings = _dbContext.Listings.Where(l => l.Status == ListingStatus.Accepted && l.Author.Company.Id == id)
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
                   ExpirationDate = l.ExpirationDate,
               }); ;
            return Ok(listings);
        }

        [HttpGet("getCertainListing/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getCertainListing([FromRoute] int id)
        {
            var listings = _dbContext.Listings.Where(l => l.Status == ListingStatus.Accepted && l.Id == id)
               .Select(l => new ListingViewModel()
               {
                   Id = l.Id,
                   Name = l.Name,
                   Description = l.Description,
                   CreatedDate = l.CreatedDate,
                   // include other properties (Authors and Categories) of the listing here
                   AuthorId = l.Author.Id,
                   AuthorName = l.Author.Company.Name,
                   CompanyId = l.Author.Company.Id,
                   CategoryNames = l.Categories.Select(c => c.Name),
                   Location = l.Location,
                   Arrangement = l.Arrangement,
                   Salary = l.Salary,
                   AuthorPic = l.Author.Company.ProfilePicture,
               }).FirstOrDefault(); ;

            return Ok(listings);
        }




        [HttpGet("getCertainCompany/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCertainCompany(int id)
        {

            var company = _dbContext.Companies.FirstOrDefault(x => x.Id == id);


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

            var result = new CompanyViewModel()
            {
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


        [HttpGet("getCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {

            var categories = _dbContext.Categories.Where(c => c.Listings.Count > 0)
                .Select(c => c.Name);

            return Ok(categories);
        }

        [HttpGet("getLocations")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocations()
        {

            var locations = _dbContext.Listings.Where(c => c.Location != "" && c.Status == ListingStatus.Accepted)
                .Select(c => c.Location).Distinct();

            return Ok(locations);
        }

        [HttpGet("GetUserCompanyId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserCompanyId([FromRoute]string id)
        {
            var user = await _dbContext.Users
             .Include(u => u.Company)
             .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return BadRequest();
            }
            

            if (user.Company != null)
            {
                return Ok(user.Company.Id);
            }

                return BadRequest();


        }

        [HttpGet("getProfile/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            if(user == null)
            {
                ModelState.AddModelError("Error", "There is no such user.");
                return BadRequest(ModelState);

            }

            var result = new ProfileViewModel()
            {
                name = user.UserName,
                description = user.Description,
                profilePicture = user.ProfilePicture,
                phoneNumber = user.PhoneNumber,
            };

            return Ok(result);

        }
    }
}
