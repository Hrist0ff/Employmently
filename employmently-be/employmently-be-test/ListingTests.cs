using employmently_be.Controllers;
using employmently_be.Data.Models;
using employmently_be.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using employmently_be.DbContexts;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Microsoft.Extensions.Options;

namespace employmently_be_test
{
    [TestClass]
    public class ListingTests
    {
        private DbContextOptions<dbContext> _dbContextOptions;

        [TestInitialize]
        public void Initialize()
        {
            // Set up a new in-memory database for each test
            _dbContextOptions = new DbContextOptionsBuilder<dbContext>()
                .Options;
        }

        [TestMethod]
        public async Task List_WithValidInput_ReturnsOkResult()
        {
            // Arrange
            var user = new User { UserName = "testuser@test.com", Email = "testuser@test.com" };
            var userManagerMock = GetUserManagerMock(user);

            var controller = new ListingController(new dbContext(_dbContextOptions), userManagerMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Company")
        }, "mock"));

            var listingDto = new ListingDto
            {
                Name = "Test Listing",
                Description = "Test Description",
                Location = "Test Location",
                Arrangement = "Test Arrangement",
                Salary = 1000,
                Categories = new List<string> { "Test Category" }
            };

            // Act
            var result = await controller.List(listingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Your Listing has been sent to admin. He should accept it and soon will be visible for everyone!", okResult.Value);

            // Verify that the listing and categories are added to the database
            using (var context = new dbContext(_dbContextOptions))
            {
                var listingInDb = await context.Listings.FirstOrDefaultAsync(l => l.Name == listingDto.Name);
                Assert.IsNotNull(listingInDb);
                Assert.AreEqual(listingDto.Description, listingInDb.Description);
                Assert.AreEqual(listingDto.Location, listingInDb.Location);
                Assert.AreEqual(listingDto.Arrangement, listingInDb.Arrangement);
                Assert.AreEqual(listingDto.Salary, listingInDb.Salary);

                var categoryInDb = await context.Categories.FirstOrDefaultAsync(c => c.Name == listingDto.Categories[0]);
                Assert.IsNotNull(categoryInDb);
            }

            using (var context = new dbContext(_dbContextOptions))
            {
                var listing = await context.Listings.FirstOrDefaultAsync(u => u.Name == "Test Listing");
                if (listing != null)
                {
                    context.Listings.Remove(listing);
                    await context.SaveChangesAsync();
                }
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        [TestMethod]
        public async Task List_WithInvalidName_ReturnsBadRequest()
        {
            // Arrange
            var user = new User { UserName = "testuser@test.com", Email = "testuser@test.com" };
            var userManagerMock = GetUserManagerMock(user);

            var controller = new ListingController(new dbContext(_dbContextOptions), userManagerMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Company")
        }, "mock"));

            var listingDto = new ListingDto
            {
                Name = "Test Listing",
                Description = "Test Description",
                Location = "Test Location",
                Arrangement = "Test Arrangement",
                Salary = 1000,
            };

            // Act
            var result = await controller.List(listingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }

        [TestMethod]
        public async Task List_WithInvalidCategories_ReturnsBadRequest()
        {
            // Arrange
            var user = new User { UserName = "testuser@test.com", Email = "testuser@test.com" };
            var userManagerMock = GetUserManagerMock(user);

            var controller = new ListingController(new dbContext(_dbContextOptions), userManagerMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Company")
            }, "mock"));

            var listingDto = new ListingDto
            {
                Description = "Test Description",
                Location = "Test Location",
                Arrangement = "Test Arrangement",
                Salary = 1000,
                Categories = new List<string> { "Test Category" }
            };

            // Act
            var result = await controller.List(listingDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }
        private Mock<UserManager<User>> GetUserManagerMock(User user)
        {
            var store = new Mock<IUserStore<User>>();
            var mgr = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<User>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<User>());
            mgr.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            return mgr;
        }
    }

}
