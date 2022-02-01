using System.Linq.Expressions;
using TO_DO.ENTİTY.Models;
using TO_DO.SERVİCE.Dtos;
namespace TO_DO.SERVİCE.Contracts
{
   public interface IUserService
   {
      Task<List<UserDto>> GetAllUsers();
      Task<UserDto> AddUser(UserDto userDto);
      Task<UserDto> GetUser(Expression<Func<User, bool>> filter);
      Task<bool> RemoveUser(string id);
      Task<UserDto> UpdateUser(UserDto userDto);
      Task<bool> SoftRemove(string id);
   }
}