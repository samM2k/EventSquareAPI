using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI.AccessControl;

/// <summary>
/// The Access Control Model for RSVPs.
/// </summary>
public class RsvpAccessControlModel : AccessControlModel<Rsvp>
{
    /// <summary>
    /// The invitations dataset.
    /// </summary>
    private DbSet<Invitation> Invitations;

    /// <summary>
    /// Constructs a new instance of the RsvpAccessControlModel class.
    /// </summary>
    /// <param name="rsvps">The RSVPs dataset.</param>
    /// <param name="invitations">The invitations dataset.</param>
    /// <param name="userManager">The user manager.</param>
    public RsvpAccessControlModel(DbSet<Rsvp> rsvps, DbSet<Invitation> invitations, UserManager<IdentityUser> userManager) : base(rsvps, userManager)
    {
        this.Invitations = invitations;
    }

    /// <summary>
    /// Gets whether the entity type has an owner field.
    /// </summary>
    public override bool EntityHasOwnership => true;

    /// <summary>
    /// Gets whether the entity type has an explicit access control mechanism.
    /// </summary>
    public override bool EntityHasExplicitAccessControl => true;

    /// <summary>
    /// Gets whether the entity has a visibility field.
    /// </summary>
    public override bool EntityHasVisibility => false;

    /// <summary>
    /// Checks whether this record is hidden.
    /// </summary>
    /// <param name="record">The rsvp.</param>
    /// <returns>N/A not implemented.</returns>
    /// <exception cref="NotImplementedException">Not applicable to class.</exception>
    public override bool CheckIfHidden(Rsvp record)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks whether this record is public.
    /// </summary>
    /// <param name="record">The rsvp.</param>
    /// <returns>N/A not implemented.</returns>
    /// <exception cref="NotImplementedException">Not applicable to class.</exception>
    public override bool CheckIfPublic(Rsvp record)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks whether the user has explicit access to (read) this record.
    /// </summary>
    /// <param name="record">The RSVP.</param>
    /// <param name="user">The user.</param>
    /// <returns>A value indicating whether or not the user has explicit read access to the record.</returns>
    /// <remarks>
    /// <para>Returns true if the requester (user) is the sender of the invite being RSVP'd to.</para>
    /// <para>Checks for an invite where eventId == rsvp.eventid, invite-sender == the RSVPer and the one who sent the invite is the current user.</para>
    /// </remarks>
    public override bool CheckIfUserHasExplicitAccess(Rsvp record, IdentityUser user)
    {
        return this.Invitations.Any(
            inv =>
            inv.SenderId == user.Id &&
            inv.ReceipientId == record.UserId &&
            inv.EventId == record.EventId);
    }

    /// <summary>
    /// Get owner Id from the record.
    /// </summary>
    /// <param name="record">The record.</param>
    /// <returns>The sender of the RSVP.</returns>
    public override string GetOwnerIdFromEntity(Rsvp record)
    {
        // returns the RSVP recipient to enable them access as an "owner".
        return record.UserId;
    }
}