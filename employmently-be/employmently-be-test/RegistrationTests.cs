using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using employmently_be.Controllers;
using Moq;
using employmently_be.Data.Models;
using employmently_be.DbContexts;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace employmently_be_test
{
    [TestClass]

    public class RegistrationTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<IEmailSender> _mockEmailSender;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<EmailSender>> _mockLogger;
        private Mock<IOptions<AuthMessageSenderOptions>> _mockMailer;


        public RegistrationTests()
        {
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null);

            _mockEmailSender = new Mock<IEmailSender>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<EmailSender>>();
            _mockMailer = new Mock<IOptions<AuthMessageSenderOptions>>();
        }


        [TestMethod]
        public async Task Register_WithValidInput_SendsConfirmationEmail()
        {


            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .Options;

            using (var context = new dbContext(options))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "newuser@test.com");
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                }

            }

            // Arrange
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager
                .Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .ReturnsAsync("confirmation_token");

            var builder = new ConfigurationBuilder()
                .AddUserSecrets<RegistrationTests>();
            var configuration = builder.Build();

            var sendGridKey = configuration["SendGridKey"];
            _mockMailer.Setup(x => x.Value).Returns(new AuthMessageSenderOptions { SendGridKey = sendGridKey });



            // Arrange
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var uri = new Uri("https://localhost:7015/");
            httpContext.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.Scheme).Returns(uri.Scheme);
            request.Setup(x => x.Host).Returns(new HostString(uri.Host, uri.Port));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("fake_url");

            var dbContext = new dbContext(options);
            var controller = new RegistrationController(dbContext, _mockUserManager.Object, _mockConfiguration.Object, _mockLogger.Object, _mockMailer.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext.Object
                },
                Url = mockUrlHelper.Object
            };


            var userToAdd = new RegisterDto()
            {
                Username = "FirstName",
                Email = "newuser@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await controller.Register(userToAdd);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            using (var context = new dbContext(options))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "newuser@test.com");
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                }
            }

        }

        [TestMethod]
        public async Task Register_WithInvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .Options;

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to create user" }));


            // Arrange
            var httpContext = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var uri = new Uri("https://localhost:7015/");
            httpContext.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.Scheme).Returns(uri.Scheme);
            request.Setup(x => x.Host).Returns(new HostString(uri.Host, uri.Port));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("fake_url");

            var dbContext = new dbContext(options);
            var controller = new RegistrationController(dbContext, _mockUserManager.Object, _mockConfiguration.Object, _mockLogger.Object, _mockMailer.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext.Object
                },
                Url = mockUrlHelper.Object
            };

            var userToAdd = new RegisterDto()
            {
                Username = "FirstName",
                Email = "", // empty email
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await controller.Register(userToAdd);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to create user", badRequestResult.Value.ToString());

        }

        [TestMethod]
        public async Task Register_WithDifferentPassword_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .Options;

            // Arrange
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Passwords do not match" }));

            var dbContext = new dbContext(options);
            var controller = new RegistrationController(dbContext, _mockUserManager.Object, _mockConfiguration.Object, _mockLogger.Object, _mockMailer.Object);

            var userToAdd = new RegisterDto()
            {
                Username = "FirstName",
                Email = "newuser@test.com",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword123!"
            };

            // Act
            var result = await controller.Register(userToAdd);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

        }

        [TestMethod]
        public async Task Register_WithExistingUser_ReturnsBadRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<dbContext>()
                .Options;

            using (var context = new dbContext(options))
            {
                context.Users.Add(new User { Email = "existingemail@test.com" });
                await context.SaveChangesAsync();
            }

            // Arrange
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var dbContext = new dbContext(options);
            var controller = new RegistrationController(dbContext, _mockUserManager.Object, _mockConfiguration.Object, _mockLogger.Object, _mockMailer.Object);

            var userToAdd = new RegisterDto()
            {
                Username = "FirstName",
                Email = "existingemail@test.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = await controller.Register(userToAdd);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);

            using (var context = new dbContext(options))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == "existingemail@test.com");
                if (user != null)
                {
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                }
            }


        }



    }
}
