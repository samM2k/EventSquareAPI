using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI.AccessControl;

/// <summary>
/// The event access control model.
/// </summary>
public class EventAccessControlModel : AccessControlModel<CalendarEvent>
{
    private DbSet<Invitation> Invitations { get; }

    /// <summary>
    /// Constructs a new instance of the EventAccessControlModel.
    /// </summary>
    /// <param name="events">The events dataset.</param>
    /// <param name="invitations">The invitations dataset.</param>
    /// <param name="userManager">The user manager.</param>
    public EventAccessControlModel(DbSet<CalendarEvent> events, DbSet<Invitation> invitations, UserManager<IdentityUser> userManager) : base(events, userManager, true, true, true)
    {
        this.Invitations = invitations;
    }

    /// <summary>
    /// Check if a record is hidden.
    /// </summary>
    /// <param name="record">The record to check</param>
    /// <returns>Whether or not the record is marked as hidden.</returns>
    public override bool CheckIfHidden(CalendarEvent record)
    {
        return record.Visibility == EventVisibility.Hidden;
    }

    /// <summary>
    /// Check if an event is public.
    /// </summary>
    /// <param name="record">The event to check,</param>
    /// <returns>Whether or not the event is marked as public.</returns>
    public override bool CheckIfPublic(CalendarEvent record)
    {
        return record.Visibility == EventVisibility.Public;
    }

    /// <summary>
    /// Checks if a user has explicit (read) access to an event.
    /// </summary>
    /// <param name="record">The event to check.</param>
    /// <param name="user">The user to check explicit access for.</param>
    /// <returns>Whether or not the user has explicit access for the event.</returns>
    public override bool CheckIfUserHasExplicitAccess(CalendarEvent record, IdentityUser user)
    {
        bool isInvited = this.Invitations.Any(a => a.ReceipientId == user.Id && a.EventId == record.Id);
        return isInvited;
    }

    /// <summary>
    /// Gets the UserId to treat as the owner of this event.
    /// </summary>
    /// <param name="record">The event.</param>
    /// <returns>The record's owner.</returns>
    public override string GetOwnerIdFromEntity(CalendarEvent record)
    {
        return record.Owner;
    }
}
