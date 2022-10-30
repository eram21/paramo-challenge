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
        /// <summary>
        /// Verifying UsersController it's OK.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Verifying UsersController when Throws Exception.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Verifying Create User in UserServices it's OK.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Verifying Create User in UserServices Fail when Email already exists.
        /// </summary>
        /// <returns></returns>
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
            Assert.Equal($"This email (Agustina@gmail.com) already exists, please try another. {Environment.NewLine}", result.Messages);
        }

        /// <summary>
        /// Verifying the money calculation of the normal user over 100 USD.
        /// </summary>
        [Fact]
        public void TestUserServicesCalculateNormalUserMoneyOver100USD()
        {
            // Arrange
            var repository = new Mock<IRepository>();

            repository.Setup(x => x.GetAllUsers()).ReturnsAsync(new List<User>());
            var user = new User
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                Type = UserType.Normal,
                Money = 110
            };

            var userServices = new UserServices(repository.Object);

            // Act
            var result = userServices.CalculateUserMoney(user);

            // Assert
            Assert.Equal((decimal)123.20, user.Money);
        }

        /// <summary>
        /// Verifying the money calculation of the normal user over 10 USD.
        /// </summary>
        [Fact]
        public void TestUserServicesCalculateNormalUserMoneyOver10USD()
        {
            // Arrange
            var repository = new Mock<IRepository>();

            repository.Setup(x => x.GetAllUsers()).ReturnsAsync(new List<User>());
            var user = new User
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                Type = UserType.Normal,
                Money = 50
            };

            var userServices = new UserServices(repository.Object);

            // Act
            var result = userServices.CalculateUserMoney(user);

            // Assert
            Assert.Equal((decimal)90, user.Money);
        }
    }
}