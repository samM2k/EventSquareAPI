using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventSquareAPI;
using EventSquareAPI.DataTypes;

namespace EventSquareAPI.Controllers;

/// <summary>
/// Controls the management of RSVP records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RsvpsController : ControllerBase
{
    /// <summary>
    /// The data context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// The RSVPs Controller.
    /// </summary>
    /// <param name="context">The data context.</param>
    public RsvpsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Rsvps
    /// <summary>
    /// GET Request for all RSVPs.
    /// </summary>
    /// <returns>The HTTP response.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rsvp>>> GetRsvps()
    {
      if (_context.Rsvps == null)
      {
          return Problem("Entity set not found in database.");
      }
        return await _context.Rsvps.ToListAsync();
    }

    // GET: api/Rsvps/5
    /// <summary>
    /// GET Request for an RSVP of a given Id.
    /// </summary>
    /// <param name="id">The Id for which to return a corresponding RSVP record.</param>
    /// <returns>The HTTP response.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Rsvp>> GetRsvp(string id)
    {
      if (_context.Rsvps == null)
      {
          return NotFound();
      }
        var rsvp = await _context.Rsvps.FindAsync(id);

        if (rsvp == null)
        {
            return NotFound();
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
    public async Task<IActionResult> PutRsvp(string id, Rsvp rsvp)
    {
        if (id != rsvp.Id)
        {
            return BadRequest();
        }

        _context.Entry(rsvp).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RsvpExists(id))
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

    // POST: api/Rsvps
    /// <summary>
    /// Insert a new RSVP.
    /// </summary>
    /// <param name="rsvp">The RSVP to insert.</param>
    /// <returns>The HTTP Response.</returns>
    [HttpPost]
    public async Task<ActionResult<Rsvp>> PostRsvp(Rsvp rsvp)
    {
      if (_context.Rsvps == null)
      {
          return Problem("Entity set 'ApplicationDbContext.Rsvps'  is null.");
      }
        _context.Rsvps.Add(rsvp);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (RsvpExists(rsvp.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetRsvp", new { id = rsvp.Id }, rsvp);
    }

    // DELETE: api/Rsvps/5
    /// <summary>
    /// Delete the RSVP corresponding to a given ID.
    /// </summary>
    /// <param name="id">The Id of the RSVP to delete.</param>
    /// <returns>The HTTP Response.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRsvp(string id)
    {
        if (_context.Rsvps == null)
        {
            return NotFound();
        }
        var rsvp = await _context.Rsvps.FindAsync(id);
        if (rsvp == null)
        {
            return NotFound();
        }

        _context.Rsvps.Remove(rsvp);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Checks whether an RSVP exists for the given id.
    /// </summary>
    /// <param name="id">The unique identifier for an RSVP.</param>
    /// <returns>A value indicating or not an RSVP exists for the given Id.</returns>
    private bool RsvpExists(string id)
    {
        return (_context.Rsvps?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
