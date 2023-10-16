using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI.Controllers;

/// <summary>
/// Controls the management of Event records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    /// <summary>
    /// The access control model
    /// </summary>
    private AccessControlModel<CalendarEvent> _accessControl;

    /// <summary>
    /// The data context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// The Events Controller.
    /// </summary>
    /// <param name="context">The data context.</param>
    /// <param name="userManager">The user manager.</param>
    public EventsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        this._context = context;
        this._accessControl = new(
            this._context.Events,
            true,
            false,
            true,
            a => a.Owner,
            null,
            a => a.Visibility == EventVisibility.Public,
            a => a.Visibility == EventVisibility.Hidden,
            userManager);
    }

    /// <summary>
    /// Get all events.
    /// </summary>
    /// <returns>The HTTP response.</returns>
    /// <remarks>
    ///     <para>
    ///         If unauthorized, only retrieve public events.
    ///     </para>
    /// 
    ///     <para>
    ///         If authorized as standard user, only retrieve public events, those the user has been invited to and those the user is owner of.
    ///     </para>
    ///     <para>
    ///         If authorized as admin user, view all events.
    ///     </para>
    /// </remarks>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
    {
        var results = this._context.Events.AsEnumerable();
        if (results == null)
        {
            return this.Problem("Entity set 'ApplicationDbContext.Events' is null.");
        }

        var returnValue = await this._accessControl.GetRecordsAsync(this.HttpContext.User);
        return this.Ok(returnValue);
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
        if (this._context.Events == null)
        {
            return this.NotFound();
        }
        var calendarEvent = await this._context.Events.FindAsync(id);

        if (calendarEvent == null)
        {
            return this.NotFound();
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
    [Authorize]
    public async Task<IActionResult> PutCalendarEvent(string id, CalendarEvent calendarEvent)
    {
        if (id != calendarEvent.Id)
        {
            return this.BadRequest("Entity Id should be consistent between provided data and URI.");
        }

        this._context.Entry(calendarEvent).State = EntityState.Modified;

        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.CalendarEventExists(id))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }

        return this.NoContent();
    }

    // POST: api/Events
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Insert a new event.
    /// </summary>
    /// <param name="calendarEvent">The new event.</param>
    /// <returns>The HTTP response.</returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CalendarEvent>> PostCalendarEvent(CalendarEvent calendarEvent)
    {
        if (this._context.Events == null)
        {
            return this.Problem("Entity set 'ApplicationDbContext.Events'  is null.");
        }
        this._context.Events.Add(calendarEvent);
        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (this.CalendarEventExists(calendarEvent.Id))
            {
                return this.Conflict("Id already exists.");
            }
            else
            {
                throw;
            }
        }

        return this.CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
    }

    // DELETE: api/Events/5
    /// <summary>
    /// Delete an event of a given Id.
    /// </summary>
    /// <param name="id">The Id of the event to delete.</param>
    /// <returns>The HTTP response.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCalendarEvent(string id)
    {
        if (this._context.Events == null)
        {
            return this.NotFound();
        }
        var calendarEvent = await this._context.Events.FindAsync(id);
        if (calendarEvent == null)
        {
            return this.NotFound();
        }

        this._context.Events.Remove(calendarEvent);
        await this._context.SaveChangesAsync();

        return this.NoContent();
    }

    /// <summary>
    /// Checks whether an event exists for a given Id.
    /// </summary>
    /// <param name="id">The Id for which to check for a corresponding event.</param>
    /// <returns>A value indicating whether or not an event exists with the provided Id.</returns>
    private bool CalendarEventExists(string id)
    {
        return (this._context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
