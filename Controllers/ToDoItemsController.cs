using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApi.Models;

namespace ToDoListApi.Controllers
{
    [Route("api/ToDoItems")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoItemsController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItemDTO>>> GetTodoItems()
        {
            return await _context.ToDoItems.
                Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemDTO>> GetToDoItem(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(toDoItem);
        }

        // PUT: api/ToDoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(long id, ToDoItemDTO toDoItemDTO)
        {
            if (id != toDoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.ToDoItems.FindAsync(id);
            if(todoItem == null)
            {
                return NotFound();
            }


            todoItem.TaskName = toDoItemDTO.TaskName;
            todoItem.IsCompleted = toDoItemDTO.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ToDoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> PutToDoItemWithoutId(ToDoItemDTO toDoItemDTO)
        {
            long id = toDoItemDTO.Id;

            var todoItem = await _context.ToDoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.TaskName = toDoItemDTO.TaskName;
            todoItem.IsCompleted = toDoItemDTO.IsCompleted;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ToDoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/ToDoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> CreateToDoItem(ToDoItemDTO toDoItemDTO)
        {

            var toDoItem = new ToDoItem
            {
                IsCompleted = toDoItemDTO.IsCompleted,
                TaskName = toDoItemDTO.TaskName
            };

            _context.ToDoItems.Add(toDoItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
            return CreatedAtAction(
                nameof(GetToDoItem),
                new { id = toDoItem.Id },
                ItemToDTO(toDoItem));
        }

        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemExists(long id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }

        private static ToDoItemDTO ItemToDTO(ToDoItem todoItem) =>
        new ToDoItemDTO
        {
            Id = todoItem.Id,
            TaskName = todoItem.TaskName,
            IsCompleted = todoItem.IsCompleted
        };
    }
}
