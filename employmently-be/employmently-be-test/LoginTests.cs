using employmently_be.Controllers;
using employmently_be.Data.Entities;
using employmently_be.Data.Models;
using employmently_be.Data.Models.ViewModels;
using employmently_be.Entities;
using employmently_be.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using employmently_be.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace employmently_be_test
{
    [TestClass]
    public class LoginTests
    {
        private LoginController _controller;
        private Mock<UserManager<User>> _userManager;
        private Mock<IConfiguration> _configuration;
        private Mock<IRefreshTokenRepository> _refreshTokenRepository;

        [TestInitialize]
        public void Setup()
        {
            _userManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);

            _configuration = new Mock<IConfiguration>();

            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();

            _controller = new LoginController(
                _userManager.Object, _configuration.Object, _refreshTokenRepository.Object);
        }

        [TestMethod]
        public async Task Login_WithValidCredentials_ReturnsAccessTokenAndRefreshToken()
        {
            // Arrange
            var input = new LoginDto { Email = "user1@test.com", Password = "password123" };

            var user = new User
            {
                Id = "user1",
                UserName = input.Email,
                Email = input.Email,
                EmailConfirmed = true
            };

            _userManager.Setup(x => x.FindByEmailAsync(input.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, input.Password))
                .ReturnsAsync(true);

            _userManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Candidate" });

            var refreshToken = "refreshToken1";
            var expirationDate = DateTime.UtcNow.AddDays(1);

            _refreshTokenRepository.Setup(x => x.GetRefreshToken(user.Id))
                .ReturnsAsync(new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.Id,
                    ExpirationDate = expirationDate
                });

            _configuration.Setup(x => x["Jwt:Key"])
                .Returns("yr5Yz2QWxxwxe9hkvu4t4RfRHIMwPiEFDv442B6v");

            _configuration.Setup(x => x["Jwt:Issuer"])
                .Returns("https://localhost:7015");

            // Act
            var result = await _controller.Login(input);

            // Assert
            var response = result.Value;
            Assert.IsNotNull(response.accessToken);
            Assert.IsNotNull(response.refreshToken);
        }

        [TestMethod]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var input = new LoginDto { Email = "user1@test.com", Password = "wrongpassword" };

            var user = new User
            {
                Id = "user1",
                UserName = input.Email,
                Email = input.Email,
                EmailConfirmed = true
            };

            _userManager.Setup(x => x.FindByEmailAsync(input.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, input.Password))
                .ReturnsAsync(false);

            _configuration.Setup(x => x["Jwt:Key"])
                .Returns("yr5Yz2QWxxwxe9hkvu4t4RfRHIMwPiEFDv442B6v");

            _configuration.Setup(x => x["Jwt:Issuer"])
                .Returns("https://localhost:7015");

            // Act
            var result = await _controller.Login(input);

            // Assert
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);
            Assert.AreEqual(400, badRequestObjectResult.StatusCode);

            var errors = badRequestObjectResult.Value as SerializableError;
            Assert.IsNotNull(errors);

            var errorMessage = errors.GetValueOrDefault("Error") as string[];
            Assert.IsNotNull(errorMessage);
            Assert.IsTrue(errorMessage.Contains("Error in password"));
        }

        [TestMethod]
        public async Task Login_WithNonExistingUser_ReturnsError()
        {
            // Arrange
            var input = new LoginDto { Email = "nonexistinguser@test.com", Password = "password123" };

            _userManager.Setup(x => x.FindByEmailAsync(input.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(input);
            
            // Assert
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);
            Assert.AreEqual(400, badRequestObjectResult.StatusCode);

            var errors = badRequestObjectResult.Value as SerializableError;
            Assert.IsNotNull(errors);

            var errorMessage = errors.GetValueOrDefault("Error") as string[];
            Assert.IsNotNull(errorMessage);
            Assert.IsTrue(errorMessage.Contains("There's no such a user"));
        }

        [TestMethod]
        public async Task Login_WithUnconfirmedEmail_ReturnsError()
        {
            // Arrange
            var input = new LoginDto { Email = "user1@test.com", Password = "password123" };

            var user = new User
            {
                Id = "user1",
                UserName = input.Email,
                Email = input.Email,
                EmailConfirmed = false
            };

            _userManager.Setup(x => x.FindByEmailAsync(input.Email))
                .ReturnsAsync(user);

            _userManager.Setup(x => x.CheckPasswordAsync(user, input.Password))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Login(input);

            // Assert
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);
            Assert.AreEqual(400, badRequestObjectResult.StatusCode);

            var errors = badRequestObjectResult.Value as SerializableError;
            Assert.IsNotNull(errors);

            var errorMessage = errors.GetValueOrDefault("Error") as string[];
            Assert.IsNotNull(errorMessage);
            Assert.IsTrue(errorMessage.Contains("Email is not confirmed! Please check your email and confirm it."));
        }

    }
    }