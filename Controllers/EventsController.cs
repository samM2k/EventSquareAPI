using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventSquareAPI;
using EventSquareAPI.DataTypes;
using System.Diagnostics;

namespace EventSquareAPI.Controllers;

/// <summary>
/// Controls the management of Event records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    /// <summary>
    /// The data context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// The Events Controller.
    /// </summary>
    /// <param name="context">The data context.</param>
    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Events
    /// <summary>
    /// Get all events.
    /// </summary>
    /// <returns>The HTTP response.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
    {
      if (_context.Events == null)
      {
          return NotFound();
      }
        return await _context.Events.ToListAsync();
    }

    // GET: api/Events/5
    /// <summary>
    /// Get an event of a given Id.
    /// </summary>
    /// <param name="id">The unique identifier of the event.</param>
    /// <returns>The HTTP response.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(string id)
    {
      if (_context.Events == null)
      {
          return NotFound();
      }
        var calendarEvent = await _context.Events.FindAsync(id);

        if (calendarEvent == null)
        {
            return NotFound();
        }

        return calendarEvent;
    }

    // PUT: api/Events/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Update a given event. 
    /// </summary>
    /// <param name="id">The unique identifier of the event.</param>
    /// <param name="calendarEvent">The updated event.</param>
    /// <returns>The HTTP response.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCalendarEvent(string id, CalendarEvent calendarEvent)
    {
        if (id != calendarEvent.Id)
        {
            return BadRequest("Entity Id should be consistent between provided data and URI.");
        }

        _context.Entry(calendarEvent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CalendarEventExists(id))
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

    // POST: api/Events
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Insert a new event.
    /// </summary>
    /// <param name="calendarEvent">The new event.</param>
    /// <returns>The HTTP response.</returns>
    [HttpPost]
    public async Task<ActionResult<CalendarEvent>> PostCalendarEvent(CalendarEvent calendarEvent)
    {
      if (_context.Events == null)
      {
          return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
      }
        _context.Events.Add(calendarEvent);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (CalendarEventExists(calendarEvent.Id))
            {
                return Conflict("Id already exists.");
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
    }

    // DELETE: api/Events/5
    /// <summary>
    /// Delete an event of a given Id.
    /// </summary>
    /// <param name="id">The Id of the event to delete.</param>
    /// <returns>The HTTP response.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCalendarEvent(string id)
    {
        if (_context.Events == null)
        {
            return NotFound();
        }
        var calendarEvent = await _context.Events.FindAsync(id);
        if (calendarEvent == null)
        {
            return NotFound();
        }

        _context.Events.Remove(calendarEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Checks whether an event exists for a given Id.
    /// </summary>
    /// <param name="id">The Id for which to check for a corresponding event.</param>
    /// <returns>A value indicating whether or not an event exists with the provided Id.</returns>
    private bool CalendarEventExists(string id)
    {
        return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
