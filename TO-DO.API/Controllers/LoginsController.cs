using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;

namespace TO_DO.API.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   [AllowAnonymous]
   public class LoginsController : ControllerBase
   {
      private readonly ITokenService _tokenService;
      private readonly IUserService _userService;

      public LoginsController(ITokenService tokenService, IUserService userService)
      {
         _tokenService = tokenService;
         _userService = userService;
      }

      [HttpPost("login")]
      public async Task<IActionResult> LoginAsync(LoginDto loginDto)
      {
         if (!string.IsNullOrEmpty(loginDto.UserName) && !string.IsNullOrEmpty(loginDto.Password))
         {
            var user = await _userService.GetUser(x => x.UserName == loginDto.UserName && x.Password == loginDto.Password);
            if (user == null)
            {
               return BadRequest("Username or password invalid.");
            }
            var token = _tokenService.GetToken(user);
            return Ok(token);
         }
         else
         {
            return BadRequest("Fill in all fields");
         }
      }
   }
}