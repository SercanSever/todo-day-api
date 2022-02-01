using Microsoft.AspNetCore.Mvc;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;

namespace TO_DO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetUser(string id)
        {
            var result = await _userService.GetUser(x => x.UserId == id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("UserId Invalid");
        }
        [HttpPost("add")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var result = await _userService.AddUser(userDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            var result = await _userService.UpdateUser(userDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.RemoveUser(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("Invalid User");
        }
        [HttpPost("softdelete")]
        public async Task<IActionResult> SoftDeleteUser(string id)
        {
            var result = await _userService.SoftRemove(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("Invalid User");
        }
    }
}