using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers.Company
{
    [Route("/Company")]
    [ApiController]

    public class CompanyController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public CompanyController(dbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        [HttpPut("changeDescription/{id}/{companyId}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> ChangeDesc([FromRoute]string id,[FromRoute]string companyId, [FromBody] string description)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }

            if(description == null)
            {
                return BadRequest("Description can't be empty.");
            }


            if (requestedUser.Id == currentUser.Id)
            {
                if(currentUser.UniqueIdentifierCompany == companyId)
                {
                    foreach (Data.Entities.Company company in _dbContext.Companies)
                    {
                        if (company.UniqueIdentifier == companyId)
                        {
                            company.Description = description;
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPut("changeYearOfCreation/{id}/{companyId}")]
        [Authorize(Roles = "Company")]
        public async Task<IActionResult> ChangeYear([FromRoute] string companyId , [FromRoute] string id , [FromBody] string yearToChange)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            int year;

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }

            if (requestedUser.Id == currentUser.Id)
            {
                if (int.TryParse(yearToChange, out year) && (year >= 1950 && year <= 2023))
                {
                    foreach (Data.Entities.Company company in _dbContext.Companies)
                    {
                        if (company.UniqueIdentifier == companyId)
                        {
                            company.YearCreated = yearToChange;
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest("Invalid year. Try year between 1950 and 2023.");
            }

            return BadRequest();
        }
    }
}
