using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TO_DO.API.Controllers;
using TO_DO.ENTİTY.Models;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;
using Xunit;

namespace TO_DO_UTEST.API
{
   public class UserControllerUnitTest
   {
      private UsersController _controller;
      private Mock<IUserService> _mock;
      private List<UserDto> _users;
      public UserControllerUnitTest()
      {
         _mock = new Mock<IUserService>();
         _controller = new UsersController(_mock.Object);
         _users = new List<UserDto>()
            {
                new UserDto
                {
                    UserName = "Test content",
                    IsActive = true,
                    UserId = "1",
                    Email = "testtestuser",
                    Password = "testpassword",
                    PasswordHash = "testpassword"
                },
                new UserDto
                {
                    UserName = "Test content",
                    IsActive = true,
                    UserId = "2",
                    Email = "testtestuser",
                    Password = "test",
                    PasswordHash = "test"
                }
            };
      }

      [Fact]
      public async void GetAllUsers_ActionExecutes_ReturnOkResultWithUsers()
      {
         _mock.Setup(x => x.GetAllUsers()).ReturnsAsync(_users);

         var result = await _controller.GetAllUsers();

         var okResult = Assert.IsType<OkObjectResult>(result);

         var returnUsers = Assert.IsAssignableFrom<List<UserDto>>(okResult.Value);

         Assert.Equal<int>(2, returnUsers.ToList().Count);
      }
      [Theory]
      [InlineData("0", "UserId Invalid")]
      public async void GetUser_IdInvalid_ReturnBadRequest(string userId, string expected)
      {
         UserDto userDto = null;
         _mock.Setup(x => x.GetUser(x => x.UserId == userId)).ReturnsAsync(userDto);

         var result = await _controller.GetUser(userId);

         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

         var returnUser = Assert.IsAssignableFrom<string>(badRequestResult.Value);

         Assert.Equal<string>(expected, returnUser);
      }
      [Theory]
      [InlineData("1")]
      public async void GetUser_IdValid_ReturnOkResult(string userId)
      {
         var user = _users.First(x => x.UserId == userId);
         _mock.Setup(x => x.GetUser(x => x.UserId == userId)).ReturnsAsync(user);

         var result = await _controller.GetUser(userId);

         var okResult = Assert.IsType<OkObjectResult>(result);

         var returnUser = Assert.IsAssignableFrom<UserDto>(okResult.Value);

         Assert.Equal(userId, returnUser.UserId);
      }
      [Theory]
      [InlineData("1")]
      public async void UpdateUser_ActionExecutes_ReturnOkResult(string userId)
      {
         var user = _users.First(x => x.UserId == userId);

         _mock.Setup(x => x.UpdateUser(user)).ReturnsAsync(user);

         var result = await _controller.UpdateUser(user);

         _mock.Verify(x => x.UpdateUser(user), Times.Once);

         var okResult = Assert.IsType<OkObjectResult>(result);
         var returnUser = Assert.IsAssignableFrom<UserDto>(okResult.Value);

         Assert.Equal(userId, user.UserId);
      }
      [Fact]
      public async void CreateUser_ActionExecutes_ReturnOkResult()
      {
         var user = _users.First();
         _mock.Setup(x => x.AddUser(user)).ReturnsAsync(user);

         var result = await _controller.CreateUser(user);

         _mock.Verify(x => x.AddUser(user), Times.Once);

         var okResult = Assert.IsType<OkObjectResult>(result);
         var returnUser = Assert.IsAssignableFrom<OkObjectResult>(result);
         Assert.Equal(user, returnUser.Value);
      }
      [Theory]
      [InlineData("0", "Invalid User")]
      public async void RemoveUser_IdInvalid_ReturnNotFound(string userId, string expected)
      {
         UserDto userDto = null;
         _mock.Setup(x => x.RemoveUser(userId)).ReturnsAsync(false);

         var result = await _controller.DeleteUser(userId);

         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

         var returnUser = Assert.IsAssignableFrom<string>(badRequestResult.Value);

         Assert.Equal<string>(expected, returnUser);
      }
      [Theory]
      [InlineData("0", "Invalid User")]
      public async void SoftRemoveUser_IdInvalid_ReturnNotFound(string userId, string expected)
      {
         UserDto userDto = null;
         _mock.Setup(x => x.SoftRemove(userId)).ReturnsAsync(false);

         var result = await _controller.SoftDeleteUser(userId);

         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

         var returnUser = Assert.IsAssignableFrom<string>(badRequestResult.Value);

         Assert.Equal<string>(expected, returnUser);
      }
   }
}