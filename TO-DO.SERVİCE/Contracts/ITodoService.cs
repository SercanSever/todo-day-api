using Couchbase.Query;
using TO_DO.ENTİTY.Models;
using TO_DO.SERVİCE.Dtos;

namespace TO_DO.SERVİCE.Contracts
{
   public interface ITodoService
   {
      List<TodoDto> GetAllTodos(string userName);
      Task<TodoDto> AddTodo(TodoDto todoDto, string userName);
      Task<TodoDto> GetTodo(string id, string userName);
      Task<bool> RemoveTodo(string id);
      Task<TodoDto> UpdateTodo(TodoDto todoDto);
      Task<bool> SoftRemove(string id);
   }
}