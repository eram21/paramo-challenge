using System;
using Sat.Recruitment.Api.Controllers;
using Xunit;
using Moq;
using Sat.Recruitment.Api.Services;
using Sat.Recruitment.Api.Models;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Data;
using System.Collections.Generic;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        [Fact]
        public async Task TestUserControllerOK()
        {
            // Arrange
            var service = new Mock<IUserServices>();
            service.Setup(x => x.Create(It.IsAny<UserDto>())).ReturnsAsync(Result.Ok("User Created"));
            var userController = new UsersController(service.Object);

            // Act
            var result = await userController.CreateUser(new UserDto());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Messages);
        }

        [Fact]
        public async Task TestUserControllerThrowsException()
        {
            // Arrange
            var service = new Mock<IUserServices>();
            service.Setup(x => x.Create(It.IsAny<UserDto>())).ThrowsAsync(new Exception("Test"));
            var userController = new UsersController(service.Object);

            // Act
            var result = await userController.CreateUser(new UserDto());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Test", result.Messages);
        }

        [Fact]
        public async Task TestUserServicesCreateOK()
        {
            // Arrange
            var repository = new Mock<IRepository>();
            var users = new List<User>();

            repository.Setup(x => x.GetAllUsers()).ReturnsAsync(users);

            var userDto = new UserDto
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var userServices = new UserServices(repository.Object);

            // Act
            var result = await userServices.Create(userDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User created.", result.Messages);
        }


        [Fact]
        public async Task TestUserServicesCreateEmailDuplicateFail()
        {
            // Arrange
            var repository = new Mock<IRepository>();

            var users = new List<User>(){ 
                new User {
                    Name = "Agustina",
                    Email = "Agustina@gmail.com",
                    Address = "Garay y Otra Calle",
                    Phone = "+534645213542",
                    Type = UserType.SuperUser,
                    Money = 112234
                }
            };

            repository.Setup(x => x.GetAllUsers()).ReturnsAsync(users);
            var userDto = new UserDto
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };

            var userServices = new UserServices(repository.Object);

            // Act
            var result = await userServices.Create(userDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("This email (Agustina@gmail.com) already exists, please try another. ", result.Messages);
        }
    }
}