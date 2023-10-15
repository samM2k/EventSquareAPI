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

namespace EventSquareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Events
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

        private bool CalendarEventExists(string id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
