using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public TodosController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            var todos = await _mongoDbService.GetTodosAsync();
            return Ok(todos);
        }

        // GET: api/todos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(string id)
        {
            var todo = await _mongoDbService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
        {
            await _mongoDbService.CreateTodoAsync(todo);
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT: api/todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(string id, Todo todo)
        {
            var existingTodo = await _mongoDbService.GetTodoByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }

            todo.Id = id;
            await _mongoDbService.UpdateTodoAsync(id, todo);
            return NoContent();
        }

        // DELETE: api/todos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(string id)
        {
            var todo = await _mongoDbService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            await _mongoDbService.DeleteTodoAsync(id);
            return NoContent();
        }
    }
}