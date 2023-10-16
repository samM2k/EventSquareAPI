using System.Diagnostics;

using EventSquareAPI.AccessControl;
using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI.Controllers
{
    /// <summary>
    /// Manages event invitations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : ControllerBase, IDisposable
    {
        /// <summary>
        /// The access control model.
        /// </summary>
        private readonly AccessControlModel<Invitation> AccessControlModel;

        /// <summary>
        /// The application data context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Gets whether resources have already been freed from memory.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// The invitations controller.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <param name="userManager">The user manager.</param>
        public InvitationsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this.AccessControlModel = new InvitationAccessControlModel(
                context.Invitations,
                userManager);
        }

        // GET: api/Invitations
        /// <summary>
        /// Get all invitations.
        /// </summary>
        /// <returns>The HTTP Response.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invitation>>> GetInvitations()
        {
            if (this._context.Invitations == null)
            {
                return this.Problem("Entity set not found in database.");
            }
            var records = await this.AccessControlModel.GetRecordsAsync(this.HttpContext.User);
            return this.Ok(records);
        }

        // GET: api/Invitations/5
        /// <summary>
        /// Get an invitation of a given Id.
        /// </summary>
        /// <param name="id">The unique identifier of the invitation to get.</param>
        /// <returns>The HTTP Response.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Invitation>> GetInvitation(string id)
        {
            if (this._context.Invitations == null)
            {
                return this.NotFound();
            }
            var invitation = await this._context.Invitations.FindAsync(id);

            if (invitation == null)
            {
                return this.NotFound();
            }

            if (!await this.AccessControlModel.CanReadAsync(invitation, this.HttpContext.User))
            {
                return this.Problem(detail: "Not authorised to read invitatino.", statusCode: 403);
            }

            return invitation;
        }

        // PUT: api/Invitations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Update an invitation of a given Id.
        /// </summary>
        /// <param name="id">The unique identifier of hte invitation to update.</param>
        /// <param name="invitation">The updated invitation.</param>
        /// <returns>The HTTP Response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvitation(string id, Invitation invitation)
        {
            if (id != invitation.Id)
            {
                return this.BadRequest();
            }

            var original = await this._context.Invitations.FindAsync(id);

            if (original == null)
            {
                return this.NotFound();
            }

            if (!await this.AccessControlModel.CanWriteAsync(original, this.HttpContext.User))
            {
                return this.Problem(detail: "Not authorised to modify Invitation.", statusCode: 403);
            }

            this._context.Entry(invitation).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.InvitationExists(id))
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

        // POST: api/Invitations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Insert a new invitation.
        /// </summary>
        /// <param name="invitation">The invitation.</param>
        /// <returns>The HTTP Response.</returns>
        [HttpPost]
        public async Task<ActionResult<Invitation>> PostInvitation(Invitation invitation)
        {
            if (this._context.Invitations == null)
            {
                return this.Problem("Entity set 'ApplicationDbContext.Invitations'  is null.");
            }

            var userIdentity = await this.AccessControlModel.GetUserFromClaimAsync(this.HttpContext.User);

            Debug.Assert(userIdentity is not null);
            invitation.SenderId = userIdentity.Id;

            this._context.Invitations.Add(invitation);
            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (this.InvitationExists(invitation.Id))
                {
                    return this.Conflict();
                }
                else
                {
                    throw;
                }
            }

            return this.CreatedAtAction("GetInvitation", new { id = invitation.Id }, invitation);
        }

        // DELETE: api/Invitations/5
        /// <summary>
        /// Delete an invitation of a given Id.
        /// </summary>
        /// <param name="id">The unique identifier of the invitation to delete.</param>
        /// <returns>The HTTP Response.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvitation(string id)
        {
            if (this._context.Invitations == null)
            {
                return this.NotFound();
            }
            var invitation = await this._context.Invitations.FindAsync(id);
            if (invitation == null)
            {
                return this.NotFound();
            }

            if (!await this.AccessControlModel.CanWriteAsync(invitation, this.HttpContext.User))
            {
                return this.Problem(detail: "Not authorised to delete invitation.", statusCode: 403);
            }

            this._context.Invitations.Remove(invitation);
            await this._context.SaveChangesAsync();

            return this.NoContent();
        }

        /// <summary>
        /// Checks whether an invitation exists for a given Id.
        /// </summary>
        /// <param name="id">The unique identifier to search the invitations for.</param>
        /// <returns>A result indicating whether such an invitation exists.</returns>
        private bool InvitationExists(string id)
        {
            return (this._context.Invitations?.Any(e => e.Id == id)).GetValueOrDefault();
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
}
