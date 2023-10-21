using System.Diagnostics;

using EventSquareAPI.AccessControl;
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
public class EventsController : ControllerBase, IDisposable
{
    /// <summary>
    /// The access control model
    /// </summary>
    private readonly AccessControlModel<CalendarEvent> AccessControlModel;


    /// <summary>
    /// The application configuration.
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// The data context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Gets whether resources have already been freed from memory.
    /// </summary>
    private bool disposedValue;

    /// <summary>
    /// The Events Controller.
    /// </summary>
    /// <param name="context">The data context.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="config">The application configuration.</param>
    public EventsController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        IConfiguration config)
    {
        this._context = context;
        this.AccessControlModel = new EventAccessControlModel(context.Events, context.Invitations, userManager);
        this._config = config;
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

        var returnValue = await this.AccessControlModel.GetRecordsAsync(this.HttpContext.User);
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

        if (!await this.AccessControlModel.CanReadAsync(calendarEvent, this.HttpContext.User))
        {
            return this.Problem(detail: "Not authorised to read event.", statusCode: 403);
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
        var original = await this._context.Events.FindAsync(id);
        if (original is null)
            return this.NotFound();

        if (id != calendarEvent.Id)
        {
            return this.BadRequest("Entity Id should be consistent between provided data and URI.");
        }

        // Check using the original in case owner is updated.
        if (true) //await this.AccessControlModel.CanWriteAsync(original, this.HttpContext.User)
        {
            calendarEvent = await this.ValidateEventUpdate(calendarEvent, original);

            foreach (var prop in calendarEvent.GetType().GetProperties())
            {
                var originalValue = prop.GetValue(original);
                var newValue = prop.GetValue(calendarEvent);
                if (originalValue != newValue)
                {
                    prop.SetValue(original, newValue);
                }

            }

            Trace.WriteLine(original);

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Trace.WriteLine("Exception occurred on updating database.");
                Trace.WriteLine(ex.Message);
                return this.Problem("Exception occurred on updating database.");
            }
            return this.Ok(original);
        }

        //return this.Problem(detail: "Not authorised to update event.", statusCode: 403);
    }

    private async Task<CalendarEvent> ValidateEventUpdate(CalendarEvent calendarEvent, CalendarEvent original)
    {
        var location = calendarEvent.Location;
        if (location == null)
            return calendarEvent;
        if (location.Coordinates is not null)
        {
            // event already has lat long, no need to geocode
            return calendarEvent;
        }

        if (original.Location is not null && this.LocationsAreEqual(original.Location, location))
        {
            location.Coordinates = original.Location.Coordinates;
            return calendarEvent;
        }


        calendarEvent.Location = await this.GeocodeLocation(location);

        return calendarEvent;
    }

    private bool LocationsAreEqual(Location left, Location right)
    {
        return this.GetAddressFromLocation(left) == this.GetAddressFromLocation(right);
    }

    private async Task<Location> GeocodeLocation(Location location)
    {
        var addressString = this.GetAddressFromLocation(location);
        var client = new HttpClient();
        UriBuilder builder = new UriBuilder("https://maps.googleapis.com/maps/api/geocode/json");
        builder.Query = $"address={Uri.EscapeDataString(addressString)}&key={Uri.EscapeDataString(this._config["googleMapsApiKey"] ?? "")}";
        var request = builder.Uri;
        try
        {
            var response = await client.GetAsync(request);
            var geocodeResultsJson = await response.Content.ReadAsStringAsync();
            GoogleGeocodeResponse? geoResponse = await response.Content.ReadFromJsonAsync<GoogleGeocodeResponse>();
            var latLong = geoResponse?.results.FirstOrDefault()?.geometry.location;

            if (latLong != null)
            {
                location.Coordinates = new(latLong.lat, latLong.lng);
            }

            return location;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return location;
        }

    }

    private string GetAddressFromLocation(Location location)
    {
        string addressString = string.Empty;
        if (location.FlatNumber is not null)
        {
            addressString += location.FlatNumber + ("/");
        }

        addressString += location.StreetNumber + " ";
        addressString += location.StreetName + ", ";
        addressString += location.Locality + ", ";
        addressString += location.StateRegion + ", ";
        addressString += location.Country;
        return addressString;
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

        var userIdentity = await this.AccessControlModel.GetUserFromClaimAsync(this.HttpContext.User);

        Debug.Assert(userIdentity is not null);
        calendarEvent.Owner = userIdentity.Id;

        if (calendarEvent.Location is not null && calendarEvent.Location.Coordinates is null)
        {
            calendarEvent.Location = await this.GeocodeLocation(calendarEvent.Location);
        }

        this._context.Events.Add(calendarEvent);
        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (this.CalendarEventExists(calendarEvent.Id))
            {
                return this.Conflict("Id already exists.");
            }
            else
            {
                Trace.WriteLine("Exception occurred on updating database.");
                Trace.WriteLine(ex.Message);
                return this.Problem("Exception occurred on updating database.");
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

        if (await this.AccessControlModel.CanWriteAsync(calendarEvent, this.HttpContext.User))
        {
            this._context.Events.Remove(calendarEvent);
            await this._context.SaveChangesAsync();
            return this.NoContent();
        }

        return this.Problem(detail: "Not authorised to delete event.", statusCode: 403);

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

    /// <summary>
    /// Disposes of the controller.
    /// </summary>
    /// <param name="disposing">Whether or not to free managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.AccessControlModel.Dispose();
            }

            this.disposedValue = true;
        }
    }

    /// <summary>
    /// Disposes of the Controller.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
