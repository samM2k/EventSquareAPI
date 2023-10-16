using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI.AccessControl;

/// <summary>
/// The access control model for Invitations.
/// </summary>
public class InvitationAccessControlModel : AccessControlModel<Invitation>
{
    /// <summary>
    /// Constructs a new instance of the InvitationAccessControlModel.
    /// </summary>
    /// <param name="invitations">The invitations dataset.</param>
    /// <param name="userManager">The user manager.</param>
    public InvitationAccessControlModel(DbSet<Invitation> invitations, UserManager<IdentityUser> userManager) : base(invitations, userManager)
    {
    }

    /// <summary>
    /// Gets a value indicating whether this entity type has ownership.
    /// </summary>
    public override bool EntityHasOwnership => true;

    /// <summary>
    /// Gets a value indicating whether this entity type has an explicit access control mechanism.
    /// </summary>
    public override bool EntityHasExplicitAccessControl => true;

    /// <summary>
    /// Gets whether this entity type has a visibility field.
    /// </summary>
    public override bool EntityHasVisibility => false;

    /// <summary>
    /// Checks if a given record is hidden.
    /// </summary>
    /// <param name="record">The record to check.</param>
    /// <returns>A value indciating whether the record is hidden.</returns>
    /// <exception cref="NotImplementedException">Not applicable to type.</exception>
    public override bool CheckIfHidden(Invitation record)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks if a given record is public.
    /// </summary>
    /// <param name="record">The record to check.</param>
    /// <returns>A value indciating whether the record is public.</returns>
    /// <exception cref="NotImplementedException">Not applicable to type.</exception>
    public override bool CheckIfPublic(Invitation record)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Checks whether a given user has explicit (read) access to a given record.
    /// </summary>
    /// <param name="record">The record to check for access to.</param>
    /// <param name="user">The user to check explicit access for.</param>
    /// <returns>Whether the user has explicit (read) access to the record.</returns>
    public override bool CheckIfUserHasExplicitAccess(Invitation record, IdentityUser user)
    {
        return record.ReceipientId == user.Id;
    }

    /// <summary>
    /// Gets the ID to treat as an owner of the record.
    /// </summary>
    /// <param name="record">The record.</param>
    /// <returns>The sender of the Invitation.</returns>
    public override string GetOwnerIdFromEntity(Invitation record)
    {
        return record.SenderId;
    }
}
