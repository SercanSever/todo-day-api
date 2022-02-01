using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Couchbase;
using Couchbase.KeyValue;
using Couchbase.Linq;
using Couchbase.Linq.Extensions;
using TO_DO.ENTİTY.Models;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;
using TO_DO.SERVİCE.Utilities.Business;

namespace TO_DO.SERVİCE.Concrete
{
   public class UserService : IUserService
   {

      private readonly IBucket _bucket;
      private readonly ICouchbaseCollection _collection;
      private readonly IUserBucketProvider _userBucketProvider;
      private readonly BucketContext _bucketContext;
      private readonly IMapper _mapper;

      public UserService(IMapper mapper, IUserBucketProvider userBucketProvider)
      {
         _userBucketProvider = userBucketProvider;
         _mapper = mapper;
         _bucket = _userBucketProvider.GetBucketAsync().GetAwaiter().GetResult();
         _collection = _bucket.DefaultCollection();
         _bucketContext = new BucketContext(_bucket);
      }
      public async Task<UserDto> AddUser(UserDto userDto)
      {
         var result = BusinessRules.Run(IsUsernameExist(userDto));
         if (!result)
         {
            throw new Exception("Username already exists");
         }
         var user = _mapper.Map<User>(userDto);
         user.UserId = Guid.NewGuid().ToString();
         user.IsActive = true;
         await _collection.InsertAsync(user.UserId.ToString(), user);
         return userDto;
      }
      public async Task<List<UserDto>> GetAllUsers()
      {
         var user = await _bucketContext.Query<User>().ToListAsync();
         var mappedUser = _mapper.Map<List<UserDto>>(user);
         return mappedUser;
      }
      public async Task<UserDto> GetUser(Expression<Func<User, bool>> filter)
      {
         var user = await _bucketContext.Query<User>().Where(filter).FirstOrDefaultAsync();
         var mappedUser = _mapper.Map<UserDto>(user);
         return mappedUser;
      }
      public async Task<UserDto> UpdateUser(UserDto userDto)
      {
         var user = _mapper.Map<User>(userDto);
         user.IsActive = true;
         var result = await _collection.ReplaceAsync<User>(user.UserId.ToString(), user);
         return userDto;
      }
      public async Task<bool> RemoveUser(string id)
      {
         var user = await GetUser(x => x.UserId == id);
         if (user == null)
         {
            return false;
         }
         await _collection.RemoveAsync(id);
         return true;
      }
      public async Task<bool> SoftRemove(string id)
      {
         var user = await GetUser(x => x.UserId == id);
         if (user == null)
         {
            return false;
         }
         var mappedUser = _mapper.Map<User>(user);
         mappedUser.IsActive = false;
         await _collection.ReplaceAsync<User>(mappedUser.UserId.ToString(), mappedUser);
         return true;
      }
      private bool IsUsernameExist(UserDto userDto)
      {
         if (GetUser(x => x.UserName == userDto.UserName) != null)
         {
            return false;
         }
         return true;
      }
   }
}