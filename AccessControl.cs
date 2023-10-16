using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI;

/// <summary>
/// Model for managing data access control, be it Role-based or entity-ownership-based.
/// </summary>
public class AccessControlModel<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Access Control Model.
    /// </summary>
    /// <param name="dataSet">The dataset containing the entity type.</param>
    /// <param name="entityHasOwnership">Whether or not the entity has an Owner field.</param>
    /// <param name="entityHasExplicitAccessControl">Whether or not the entity has explicit access control.</param>
    /// <param name="entityHasVisibility">Whether or not the entity has a visibility field.</param>
    /// <param name="getOwnerIdFromEntity">Function to get owner Id from the entity.</param>
    /// <param name="checkIfPublic">Function to check if entity is public.</param>
    /// <param name="checkIfHidden">Function to check if entity is hidden.</param>
    /// <param name="getUsersWithExplicitAccess">Function to get users with explicit access permission.</param>
    /// <param name="userManager"></param>
    public AccessControlModel(
        DbSet<TEntity> dataSet,
        bool entityHasOwnership,
        bool entityHasExplicitAccessControl,
        bool entityHasVisibility,
        Func<TEntity, string>? getOwnerIdFromEntity,
        Func<TEntity, List<string>>? getUsersWithExplicitAccess,
        Func<TEntity, bool>? checkIfPublic,
        Func<TEntity, bool>? checkIfHidden,
        UserManager<IdentityUser> userManager)
    {
        this.DataSet = dataSet;
        this.EntityHasOwnership = entityHasOwnership;
        this.EntityHasExplicitAccessControl = entityHasExplicitAccessControl;
        this.EntityHasVisibility = entityHasVisibility;
        this.GetOwnerIdFromEntity = getOwnerIdFromEntity;
        this.CheckIfPublic = checkIfPublic;
        this.CheckIfHidden = checkIfHidden;
        this.GetUsersWithExplicitAccess = getUsersWithExplicitAccess;
        this.UserManager = userManager;
    }

    /// <summary>
    /// The dataset for the provided entity type.
    /// </summary>
    private DbSet<TEntity> DataSet { get; init; }

    /// <summary>
    /// Whether the entity has an owner field.
    /// </summary>
    private bool EntityHasOwnership { get; init; }

    /// <summary>
    /// Whether the entity has another way of assigning access to specific users.
    /// </summary>
    private bool EntityHasExplicitAccessControl { get; init; }

    /// <summary>
    /// Whether the entity has a visibility field (public, hidden, etc.)
    /// </summary>
    private bool EntityHasVisibility { get; init; }
    private Func<TEntity, string>? GetOwnerIdFromEntity { get; init; }
    private Func<TEntity, bool>? CheckIfPublic { get; init; }
    private Func<TEntity, bool>? CheckIfHidden { get; init; }
    private Func<TEntity, List<string>>? GetUsersWithExplicitAccess { get; init; }
    private UserManager<IdentityUser> UserManager { get; init; }

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

    private async Task<string[]> GetRolesFromIdentityAsync(IdentityUser? userIdentity)
    {
        string[] userRoles = Array.Empty<string>();

        if (userIdentity is not null)
        {
            userRoles = (await this.UserManager.GetRolesAsync(userIdentity)).ToArray();
        }

        return userRoles;
    }

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

    private bool CanRead(TEntity record, IdentityUser? user, string[] userRoles)
    {
        bool isHidden = false;
        if (this.EntityHasVisibility)
        {
            isHidden = this.CheckIfHidden!(record);

            bool isPublic = this.CheckIfPublic!(record);
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
            var isOwner = this.GetOwnerIdFromEntity!(record) == user.Id;
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
            bool userHasExplicitAccess = this.GetUsersWithExplicitAccess!(record).Contains(user.Id);
            if (userHasExplicitAccess)
            {
                // User has explicit permission to access.
                return true;
            }
        }

        return false;
    }
}
