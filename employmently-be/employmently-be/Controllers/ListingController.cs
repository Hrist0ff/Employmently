using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            public async Task<ActionResult> List(ListingDto ldt)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var user = await _userManager.GetUserAsync(User);
                var listingToAdd = new Listing()
                {
                        Name = ldt.Name,
                        Description = ldt.Description,
                        CreatedDate = DateTime.Now,
                        Author = user
                };
                var flag = 0;

                
                foreach (string categoryToAdd in ldt.Categories)
                {
                  flag = 0;
                  foreach(Category existingCategories in _dbContext.Categories)
                    {
                        if(existingCategories.Name.ToLower() == categoryToAdd.ToLower())
                        {
                           existingCategories.Listings.Add(listingToAdd);
                           flag = 1;
                        }   
                    }
                    if (flag == 0)
                    {
                    var newCategory = new Category()
                        {
                            Name = categoryToAdd
                        };
                        newCategory.Listings.Add(listingToAdd);
                        _dbContext.Categories.Add(newCategory);
                }
            }
           
                _dbContext.Listings.Add(listingToAdd);
                
                _dbContext.SaveChanges();
                return Ok("Your Listing has been sent to admin. He should accept it and soon will be visible for everyone!");


            }

        }
    }

