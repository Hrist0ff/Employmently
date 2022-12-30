using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace employmently_be.Controllers
{
    [Route("/Profile")]
    [ApiController]


    public class ProfileController : Controller
    {
        private readonly dbContext _dbContext;
        private readonly UserManager<User> _userManager;
        // "DefaultEndpointsProtocol=https;AccountName=employmentlystorage;AccountKey=tQbLLmAfixQIMKPgvlmporcKOUaJ4phqihnrdOlm0450u9bm5iQ/HZ7/+PQ3QKc4wI6xfdIHKxDt+ASthlVoeQ==;EndpointSuffix=core.windows.net";
        // "fileupload" 

        public ProfileController(dbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPut("changeDescription/{id}")]
        [Authorize(Roles = "Candidate,Company")]
        public async Task<IActionResult> ChangeDesc(string id, [FromBody]string description)
        {
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }


            if(requestedUser.Id == currentUser.Id)
            {
                currentUser.Description = description;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("changePhoneNumber/{id}")]
        [Authorize(Roles = "Candidate,Company")]
        public async Task<IActionResult> changePhoneNumber(string id, [FromBody] string number)
        {
            Console.WriteLine(number);
            var requestedUser = await _userManager.FindByIdAsync(id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || requestedUser == null)
            {
                return NotFound();
            }

            if (requestedUser.Id == currentUser.Id)
            {
                currentUser.PhoneNumber = number;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("uploadPic/{id}")]
        [Authorize(Roles = "Company,Candidate")]
        public async Task<IActionResult> PostImage([FromRoute]string id,IFormFile image)
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
            Console.WriteLine("curr" + currentUser);
            
            if (currentUser == null || requestedUser == null)
            {
                Console.WriteLine("notfound");
                return NotFound();
            }


            if (requestedUser.Id == currentUser.Id)
            {
                string imageName = currentUser.Id + "_profilepic" + Path.GetExtension(image.FileName);
                Console.WriteLine(imageName);
                BlobClient blobClient = containerClient.GetBlobClient(imageName);
                using (var stream = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
                }
                currentUser.ProfilePicture = blobClient.Uri.AbsoluteUri;
                await _dbContext.SaveChangesAsync();
                return Ok(blobClient.Uri.AbsoluteUri);
            }
            return BadRequest();
        }


    }
}