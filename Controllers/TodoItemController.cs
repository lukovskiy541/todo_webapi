using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_webapi.Core.Entities;
using TodoApp.Application.Interfaces;

namespace todo_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;

        public TodoItemController(IToDoItemService toDoItemService)
        {
            this._toDoItemService = toDoItemService;
        }

        // GET: api/TodoItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        {
            return Ok(await _toDoItemService.GetAllAsync());
        }

        // GET: api/TodoItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetById(int id)
        {
            var item = await _toDoItemService.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // POST: api/TodoItem
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Create(TodoItem item)
        {
            await _toDoItemService.AddItemAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // PUT: api/TodoItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _toDoItemService.UpdateItemAsync(item);
            return NoContent();
        }

        // DELETE: api/TodoItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _toDoItemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
