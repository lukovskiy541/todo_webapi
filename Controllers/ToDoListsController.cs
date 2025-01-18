using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Interfaces;
using TodoApp.Domain;

namespace todo_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListsController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;

        public ToDoListsController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToDoList( TodoList toDoList)
        {
            await _toDoListService.AddListAsync(toDoList);
            return CreatedAtAction(nameof(GetToDoList), new { id = toDoList.Id }, toDoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoList(int id)
        {
            var toDoList = await _toDoListService.GetByIdAsync(id);
            return toDoList == null ? NotFound() : Ok(toDoList);
        }
    }
}
