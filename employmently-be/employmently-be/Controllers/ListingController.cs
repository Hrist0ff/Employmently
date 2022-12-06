using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace employmently_be.Controllers
{
        [Route("/Listing")]
        [ApiController]
        public class ListingController : Controller
        {
            private readonly dbContext _dbContext;
            private readonly UserManager<User> _userManager;

            public ListingController(dbContext dbContext, UserManager<User> userManager)
            {
                _dbContext = dbContext;
                _userManager = userManager;

            }


        [HttpPost]
            [Authorize(Roles = "Company")]
            public async Task<ActionResult> List(ListingDto rdt)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var user = await _userManager.GetUserAsync(User);
                var listingToAdd = new Listing()
                {
                    Name = rdt.Name,
                    Description = rdt.Description,
                    CreatedDate = DateTime.Now,
                    Author = user
                };

                _dbContext.Listings.Add(listingToAdd);
                
                _dbContext.SaveChanges();
                return Ok(listingToAdd);


            }

        }
    }

