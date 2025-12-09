using MongoDB.Driver;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Todo> _todos;

        public MongoDbService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
            _todos = database.GetCollection<Todo>("todos");
        }

        public async Task<List<Todo>> GetTodosAsync() =>
            await _todos.Find(_ => true).ToListAsync();

        public async Task<Todo> GetTodoByIdAsync(string id) =>
            await _todos.Find(todo => todo.Id == id).FirstOrDefaultAsync();

        public async Task CreateTodoAsync(Todo todo) =>
            await _todos.InsertOneAsync(todo);

        public async Task UpdateTodoAsync(string id, Todo todo) =>
            await _todos.ReplaceOneAsync(t => t.Id == id, todo);

        public async Task DeleteTodoAsync(string id) =>
            await _todos.DeleteOneAsync(todo => todo.Id == id);
    }
}