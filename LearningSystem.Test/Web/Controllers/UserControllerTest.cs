using FluentAssertions;
using LearningSystem.Data.Models;
using LearningSystem.Services;
using LearningSystem.Services.Models;
using LearningSystem.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LearningSystem.Test.Web.Controllers
{
    public class UserControllerTest
    {
        [Fact]
        public void DownloadCertificatesShouldBeForAuthorizedUsers()
        {
            // Arrange
            var method = typeof(UsersController).GetMethod(nameof(UsersController.DownloadCertificate));

            // Act
            var attributes = method.GetCustomAttributes(true);

            // Assert
            attributes
                .Should()
                .Match(attr => attr.Any(a => a.GetType() == typeof(AuthorizeAttribute)));

        }

        [Fact]
        public async Task ProfileShouldReturnNotFoundWithInvalidUsername()
        {
            // Arrange
            var userManager = GetUserManager();

            var controller = new UsersController(null, userManager.Object);

            // Act
            var result = await controller.Profile("Username");

            // Assert   
            result
                .Should()
                .BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ProfileShouldReturnViewWithValidModelWithValidUsername()
        {
            // Arrange
            const string userId = "SomeId";
            const string username = "SomeUsername";

            var userManager = GetUserManager();
            userManager.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { Id = userId });

            var userService = new Mock<IUserService>();
            userService
                .Setup(u => u.ProfileAsync(It.Is<string>(id=> id==userId)))
                .ReturnsAsync(new UserProfileServiceModel { UserName = username });

            var controller = new UsersController(userService.Object, userManager.Object);

            // Act
            var result = await controller.Profile(username);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .Match(m => m.As<UserProfileServiceModel>().UserName == username);
            }

        private Mock<UserManager<User>> GetUserManager()
            => new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
    }
}
