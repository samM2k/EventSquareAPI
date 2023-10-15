using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventSquareAPI;
using EventSquareAPI.DataTypes;

namespace EventSquareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RsvpsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RsvpsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rsvps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rsvp>>> GetRsvps()
        {
          if (_context.Rsvps == null)
          {
              return NotFound();
          }
            return await _context.Rsvps.ToListAsync();
        }

        // GET: api/Rsvps/5
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

        private bool RsvpExists(string id)
        {
            return (_context.Rsvps?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
