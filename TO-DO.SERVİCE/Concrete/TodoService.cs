using AutoMapper;
using Couchbase;
using Couchbase.KeyValue;
using TO_DO.SERVİCE.Contracts;
using TO_DO.SERVİCE.Dtos;
using TO_DO.ENTİTY.Models;
using Couchbase.Linq;
using Couchbase.Linq.Extensions;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Query;
using System.Linq;

namespace TO_DO.SERVİCE.Concrete
{
   public class TodoService : ITodoService
   {

      private readonly IBucket _bucket;
      private readonly ICouchbaseCollection _collection;
      private readonly ITodoBucketProvider _todoBucketProvider;
      private readonly BucketContext _bucketContext;
      private readonly IMapper _mapper;

      public TodoService(IMapper mapper, ITodoBucketProvider todoBucketProvider)
      {
         _todoBucketProvider = todoBucketProvider;
         _mapper = mapper;
         _bucket = _todoBucketProvider.GetBucketAsync().GetAwaiter().GetResult();
         _collection = _bucket.DefaultCollection();
         _bucketContext = new BucketContext(_bucket);

      }
      public async Task<TodoDto> AddTodo(TodoDto todoDto, string userName)
      {
         var todo = _mapper.Map<Todo>(todoDto);
         todo.TodoId = Guid.NewGuid().ToString();
         todo.WriteDate = DateTime.Today;
         todo.IsActive = true;
         todo.UserName = userName;

         await _collection.InsertAsync(todo.TodoId.ToString(), todo);
         return todoDto;
      }
      public List<TodoDto> GetAllTodos(string userName)
      {
         var todo = _bucketContext.Query<Todo>().Where(x => x.UserName == userName).ToList();
         var mappedTodo = _mapper.Map<List<TodoDto>>(todo);
         return mappedTodo;
      }
      public async Task<TodoDto> GetTodo(string id, string userName)
      {
         var todo = await _bucketContext.Query<Todo>().Where(x => x.UserName == userName).FirstOrDefaultAsync(x => x.TodoId == id);
         var mappedTodo = _mapper.Map<TodoDto>(todo);
         return mappedTodo;
      }
      public async Task<TodoDto> UpdateTodo(TodoDto todoDto)
      {
         var todo = _mapper.Map<Todo>(todoDto);
         todo.UpdateDate = DateTime.Today;
         todo.IsActive = true;
         var result = await _collection.ReplaceAsync<Todo>(todo.TodoId.ToString(), todo);
         return todoDto;
      }
      public async Task<bool> RemoveTodo(string id)
      {
         await _collection.RemoveAsync(id);
         return true;
      }
      public async Task<bool> SoftRemove(string id)
      {
         var todo = await _collection.GetAsync(id);
         var mappedTodo = _mapper.Map<Todo>(todo.ContentAs<Todo>());
         mappedTodo.IsActive = false;
         await _collection.ReplaceAsync<Todo>(mappedTodo.TodoId.ToString(), mappedTodo);
         return true;
      }
   }
}