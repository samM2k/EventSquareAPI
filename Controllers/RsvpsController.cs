using EventSquareAPI.AccessControl;
using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI.Controllers;

/// <summary>
/// Controls the management of RSVP records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RsvpsController : ControllerBase, IDisposable
{
    private readonly AccessControlModel<Rsvp> AccessControlModel;

    /// <summary>
    /// The data context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Gets whether resources have already been freed from memory.
    /// </summary>
    private bool disposedValue;

    /// <summary>
    /// The RSVPs Controller.
    /// </summary>
    /// <param name="context">The data context.</param>
    /// <param name="userManager">The user manager.</param>
    public RsvpsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        this._context = context;
        this.AccessControlModel = new RsvpAccessControlModel(context.Rsvps, context.Invitations, userManager);
    }

    // GET: api/Rsvps
    /// <summary>
    /// Get all RSVPs.
    /// </summary>
    /// <returns>The HTTP response.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rsvp>>> GetRsvps()
    {
        if (this._context.Rsvps == null)
        {
            return this.Problem("Entity set not found in database.");
        }

        var result = await this.AccessControlModel.GetRecordsAsync(this.HttpContext.User);
        return this.Ok(result);
    }

    // GET: api/Rsvps/5
    /// <summary>
    /// Get an RSVP of a given Id.
    /// </summary>
    /// <param name="id">The Id for which to return a corresponding RSVP record.</param>
    /// <returns>The HTTP response.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Rsvp>> GetRsvp(string id)
    {
        if (this._context.Rsvps == null)
        {
            return this.NotFound();
        }
        var rsvp = await this._context.Rsvps.FindAsync(id);

        if (rsvp == null)
        {
            return this.NotFound();
        }

        return rsvp;
    }

    // PUT: api/Rsvps/5
    /// <summary>
    /// Update a given RSVP Record.
    /// </summary>
    /// <param name="id">The unique identifier of the RSVP to update.</param>
    /// <param name="rsvp">The updated RSVP.</param>
    /// <returns>The HTTP response.</returns>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutRsvp(string id, Rsvp rsvp)
    {
        if (id != rsvp.Id)
        {
            return this.BadRequest();
        }

        this._context.Entry(rsvp).State = EntityState.Modified;

        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.RsvpExists(id))
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

    // POST: api/Rsvps
    /// <summary>
    /// Insert a new RSVP.
    /// </summary>
    /// <param name="rsvp">The RSVP to insert.</param>
    /// <returns>The HTTP Response.</returns>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Rsvp>> PostRsvp(Rsvp rsvp)
    {
        if (this._context.Rsvps == null)
        {
            return this.Problem("Entity set 'ApplicationDbContext.Rsvps'  is null.");
        }
        this._context.Rsvps.Add(rsvp);
        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (this.RsvpExists(rsvp.Id))
            {
                return this.Conflict();
            }
            else
            {
                throw;
            }
        }

        return this.CreatedAtAction("GetRsvp", new { id = rsvp.Id }, rsvp);
    }

    // DELETE: api/Rsvps/5
    /// <summary>
    /// Delete the RSVP corresponding to a given ID.
    /// </summary>
    /// <param name="id">The Id of the RSVP to delete.</param>
    /// <returns>The HTTP Response.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteRsvp(string id)
    {
        if (this._context.Rsvps == null)
        {
            return this.NotFound();
        }
        var rsvp = await this._context.Rsvps.FindAsync(id);
        if (rsvp == null)
        {
            return this.NotFound();
        }

        this._context.Rsvps.Remove(rsvp);
        await this._context.SaveChangesAsync();

        return this.NoContent();
    }

    /// <summary>
    /// Checks whether an RSVP exists for the given id.
    /// </summary>
    /// <param name="id">The unique identifier for an RSVP.</param>
    /// <returns>A value indicating or not an RSVP exists for the given Id.</returns>
    private bool RsvpExists(string id)
    {
        return (this._context.Rsvps?.Any(e => e.Id == id)).GetValueOrDefault();
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
