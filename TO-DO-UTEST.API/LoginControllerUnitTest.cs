using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TO_DO.API.Controllers;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;
using Xunit;

namespace TO_DO_UTEST.API
{
   public class LoginControllerUnitTest
   {
      private LoginsController _controller;
      private Mock<IUserService> _mockUser;
      private Mock<ITokenService> _mockToken;
      private LoginDto _emptyLoginDto;
      private LoginDto _loginDto;
      private List<UserDto> _users;
      public LoginControllerUnitTest()
      {
         _mockUser = new Mock<IUserService>();
         _mockToken = new Mock<ITokenService>();
         _controller = new LoginsController(_mockToken.Object, _mockUser.Object);
         _loginDto = new LoginDto()
         {
            UserName = "test",
            Password = "test"
         };
         _emptyLoginDto = new LoginDto()
         {
            UserName = "",
            Password = ""
         };
         _users = new List<UserDto>()
            {
                new UserDto
                {
                    UserName = "TestUser",
                    IsActive = true,
                    UserId = "1",
                    Email = "testtestuser",
                    Password = "testpassword",
                    PasswordHash = "testpassword"
                },
                new UserDto
                {
                    UserName = "test",
                    IsActive = true,
                    UserId = "2",
                    Email = "testtestuser",
                    Password = "test",
                    PasswordHash = "test"
                }
            };
      }
      [Fact]
      public async void Login_UsernameAndPasswordInvalid_ReturnBadRequest()
      {
         var result = await _controller.LoginAsync(_emptyLoginDto);
         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
         var returnResult = Assert.IsAssignableFrom<string>(badRequestResult.Value);
         Assert.Equal<string>("Fill in all fields", returnResult);
      }
      [Fact]
      public async void Login_UsernameAndPasswordNotCurrect_ReturnBadRequest()
      {
         UserDto userDto = null;
         _mockUser.Setup(x => x.GetUser(x => x.UserName == _loginDto.UserName)).ReturnsAsync(userDto);
         _mockUser.Setup(x => x.GetUser(x => x.Password == _loginDto.Password)).ReturnsAsync(userDto);
         var result = await _controller.LoginAsync(_loginDto);
         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
         var returnResult = Assert.IsAssignableFrom<string>(badRequestResult.Value);
         Assert.Equal<string>("Username or password invalid.", returnResult);
      }
   }
}