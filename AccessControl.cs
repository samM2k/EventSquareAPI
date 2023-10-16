using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI;

/// <summary>
/// Model for managing data access control, be it Role-based or entity-ownership-based.
/// </summary>
public abstract class AccessControlModel<TEntity>
    where TEntity : class
{
    /// <summary>
    /// The dataset for the provided entity type.
    /// </summary>
    private DbSet<TEntity> DataSet { get; init; }

    /// <summary>
    /// Whether the entity has an owner field.
    /// </summary>
    private readonly bool EntityHasOwnership;

    /// <summary>
    /// Whether the entity has another way of assigning access to specific users.
    /// </summary>
    private readonly bool EntityHasExplicitAccessControl;

    /// <summary>
    /// Whether the entity has a visibility field (public, hidden, etc.)
    /// </summary>
    private readonly bool EntityHasVisibility;

    /// <summary>
    /// Retrieve the owner's userID from a record.
    /// </summary>
    /// <remarks>Only applicable if EntityHasOwnership</remarks>
    public abstract string GetOwnerIdFromEntity(TEntity record);

    /// <summary>
    /// A function to check if a record is public.
    /// </summary>
    /// <remarks>Only applicable if EntityHasVisibility == true</remarks>
    public abstract bool CheckIfPublic(TEntity record);

    /// <summary>
    /// A function to check if a record is hidden to all those except its owner.
    /// </summary>
    /// <remarks>Only applicable if EntityHasVisibility == true</remarks>
    public abstract bool CheckIfHidden(TEntity record);

    /// <summary>
    /// A function to retrieve a list of users with explicit access to a record.
    /// </summary>
    /// <remarks>Only applicable if EntityHasExplicitAccess == true</remarks>
    public abstract bool CheckIfUserHasExplicitAccess(TEntity record, IdentityUser user);

    /// <summary>
    /// The user manager.
    /// </summary>
    private UserManager<IdentityUser> UserManager { get; init; }


    /// <summary>
    /// Access Control Model.
    /// </summary>
    /// <param name="dataSet">The dataset containing the entity type.</param>
    /// <param name="userManager"></param>
    public AccessControlModel(
        DbSet<TEntity> dataSet,
        UserManager<IdentityUser> userManager)
    {
        this.DataSet = dataSet;
        this.UserManager = userManager;
    }

    /// <summary>
    /// Gets the records accessible to the given user.
    /// </summary>
    /// <param name="user">The user requesting the records.</param>
    /// <returns>The filtered records.</returns>
    public async Task<IEnumerable<TEntity>> GetRecordsAsync(ClaimsPrincipal? user)
    {
        IdentityUser? userIdentity = await this.GetUserFromClaimAsync(user);
        string[] userRoles = await this.GetRolesFromIdentityAsync(userIdentity);

        var results = this.DataSet.AsEnumerable().Where(a => this.CanRead(a, userIdentity, userRoles));
        return results;
    }

    /// <summary>
    /// Checks whether a given user can read a given record.
    /// </summary>
    /// <param name="record">The record being checked.</param>
    /// <param name="user">The user being checked.</param>
    /// <returns>A value indicating whether the user can read this record.</returns>
    public async Task<bool> CanReadAsync(TEntity record, ClaimsPrincipal user)
    {
        var userIdentity = await this.UserManager.GetUserAsync(user);
        string[] userRoles = await this.GetRolesFromIdentityAsync(userIdentity);
        return this.CanRead(record, userIdentity, userRoles);
    }

    /// <summary>
    /// Get a list of roles from a nullable userIdentity object.
    /// </summary>
    /// <param name="userIdentity">The user to get roles for.</param>
    /// <returns>The user's roles.</returns>
    private async Task<string[]> GetRolesFromIdentityAsync(IdentityUser? userIdentity)
    {
        string[] userRoles = Array.Empty<string>();

        if (userIdentity is not null)
        {
            userRoles = (await this.UserManager.GetRolesAsync(userIdentity)).ToArray();
        }

        return userRoles;
    }

    /// <summary>
    /// Get the UserIdentity object associated with a given ClaimsPrincipal.
    /// </summary>
    /// <param name="user">The User ClaimsPrincipal/</param>
    /// <returns>The User Identity.</returns>
    private async Task<IdentityUser?> GetUserFromClaimAsync(ClaimsPrincipal? user)
    {
        IdentityUser? userIdentity = null;
        if (user != null)
        {
            userIdentity = await this.UserManager.GetUserAsync(user);
        }

        return userIdentity;
    }

    /// <summary>
    /// Check if a user in a given set of roles can read a given record.
    /// </summary>
    /// <param name="record">The record to check access for.</param>
    /// <param name="user">The user to check access fro.</param>
    /// <param name="userRoles">The user's roles.</param>
    /// <returns></returns>
    private bool CanRead(TEntity record, IdentityUser? user, string[] userRoles)
    {
        bool isHidden = false;
        if (this.EntityHasVisibility)
        {
            isHidden = this.CheckIfHidden(record);

            bool isPublic = this.CheckIfPublic(record);
            if (isPublic)
            {
                // Everyone else gets access if public.
                return true;
            }
        }

        if (user is null)
        {
            return false;
        }


        if (userRoles.Contains("admin"))
        {
            return true;
        }


        if (this.EntityHasOwnership)
        {
            var isOwner = this.GetOwnerIdFromEntity(record) == user.Id;
            if (isOwner)
            {
                // Owner gets access regardless of anything else.
                return true;
            }
        }

        if (isHidden)
        {
            // Nobody else gets access if hidden.
            return false;
        }


        if (this.EntityHasExplicitAccessControl)
        {
            bool userHasExplicitAccess = this.CheckIfUserHasExplicitAccess(record, user);
            if (userHasExplicitAccess)
            {
                // User has explicit permission to access.
                return true;
            }
        }

        return false;
    }
}
