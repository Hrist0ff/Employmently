using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using employmently_be.Data.Models.ViewModels;
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
            };

            return Ok(result);
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
                Console.WriteLine("notfound");
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

    }
}
