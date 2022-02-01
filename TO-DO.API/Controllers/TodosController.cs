using Microsoft.AspNetCore.Mvc;
using TO_DO.SERVİCE.Dtos;
using TO_DO.SERVİCE.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace TO_DO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodosController : ControllerBase
    {
        private ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }
        [HttpGet("getall")]
        public IActionResult GetAllTodos()
        {
            var result = _todoService.GetAllTodos(UserIdentifier());
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetTodo(string id)
        {
            var result = await _todoService.GetTodo(id, UserIdentifier());
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> CreateTodo([FromBody] TodoDto todoDto)
        {
            var result = await _todoService.AddTodo(todoDto, UserIdentifier());
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTodo([FromBody] TodoDto todoDto)
        {
            var result = await _todoService.UpdateTodo(todoDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTodo(string id)
        {
            var result = await _todoService.RemoveTodo(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPost("softdelete")]
        public async Task<IActionResult> SoftDeleteTodo(string id)
        {
            var result = await _todoService.SoftRemove(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        private string UserIdentifier()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        }
    }
}