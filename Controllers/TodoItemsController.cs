using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todo_webapi.Data;
using todo_webapi.Models;

namespace todo_webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.Include(t => t.Category).ToListAsync();
    }

    // GET: api/items/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
    {
        var item = await _context.TodoItems.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);

        if (item == null)
        {
            return NotFound();
        }

        return item;
    }

    // POST: api/items
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
    {
      
        var category = await _context.Categories.FindAsync(item.CategoryId);
        if (category == null)
        {
            return NotFound(new { Message = "Category not found" });
        }

       
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();

        
        var createdItem = await _context.TodoItems
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == item.Id);

        return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, createdItem);
    }

    // PUT: api/items/5
    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> PutTodoItem(int id, TodoItem item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TodoItems.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        var updatedItem = await _context.TodoItems.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        return Ok(updatedItem);

    }

    // DELETE: api/items/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
    {
        var book = await _context.TodoItems.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();

    }
}


