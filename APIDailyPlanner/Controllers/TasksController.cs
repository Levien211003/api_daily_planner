using APIDailyPlanner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDailyPlanner.Models; // Import model của bạn

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/tasks/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<APIDailyPlanner.Models.Task>>> GetTasksByUser(int userId) // Sử dụng tên đầy đủ của class
    {
        var tasks = await _context.Tasks.Where(t => t.UserID == userId).ToListAsync();
        if (tasks == null || !tasks.Any())
        {
            return NotFound("No tasks found for the user.");
        }
        return Ok(tasks);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<APIDailyPlanner.Models.Task>> AddTask([FromBody] APIDailyPlanner.Models.Task task) // Sử dụng tên đầy đủ của class
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTaskById), new { id = task.TaskID }, task);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<APIDailyPlanner.Models.Task>> GetTaskById(int id) // Sử dụng tên đầy đủ của class
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> EditTask(int id, [FromBody] APIDailyPlanner.Models.Task task) // Sử dụng tên đầy đủ của class
    {
        if (id != task.TaskID)
        {
            return BadRequest();
        }

        task.UpdatedAt = DateTime.Now;
        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/tasks/{id}/priority
    [HttpPut("{id}/priority")]
    public async Task<IActionResult> UpdatePriority(int id, [FromBody] int priority) // Nhận priority từ body
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.Priority = priority; // Cập nhật ưu tiên
        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // PUT: api/tasks/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string newStatus) // Nhận status mới từ body
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.Status = newStatus; // Cập nhật trạng thái
        task.UpdatedAt = DateTime.Now; // Cập nhật thời gian sửa đổi
        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.TaskID == id);
    }
}
