using APIDailyPlanner.Data;
using APIDailyPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReminderController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReminderController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/reminder
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskReminder>>> GetTaskReminders()
    {
        return await _context.TaskReminders.ToListAsync();
    }

    // GET: api/reminder/reminderId/{reminderId}
    [HttpGet("reminderId/{reminderId}")]
    public async Task<ActionResult<TaskReminder>> GetReminderById(int reminderId)
    {
        var reminder = await _context.TaskReminders.FindAsync(reminderId);

        if (reminder == null)
        {
            return NotFound();
        }

        return reminder;
    }

    // POST: api/reminder
    [HttpPost]
    public async Task<ActionResult<TaskReminder>> CreateReminder(TaskReminder reminder)
    {
        _context.TaskReminders.Add(reminder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReminderById), new { reminderId = reminder.ReminderID }, reminder);
    }

    // PUT: api/reminder/{reminderId}
    [HttpPut("{reminderId}")]
    public async Task<IActionResult> UpdateReminder(int reminderId, TaskReminder reminder)
    {
        if (reminderId != reminder.ReminderID)
        {
            return BadRequest();
        }

        _context.Entry(reminder).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReminderExists(reminderId))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/reminder/{reminderId}
    [HttpDelete("{reminderId}")]
    public async Task<IActionResult> DeleteReminder(int reminderId)
    {
        var reminder = await _context.TaskReminders.FindAsync(reminderId);
        if (reminder == null)
        {
            return NotFound();
        }

        _context.TaskReminders.Remove(reminder);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReminderExists(int id)
    {
        return _context.TaskReminders.Any(e => e.ReminderID == id);
    }
}
