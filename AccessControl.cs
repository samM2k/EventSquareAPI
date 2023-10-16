﻿using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI;

/// <summary>
/// Manages data access control, be it Role-based or entity-ownership-based.
/// </summary>
public class AccessControl<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Constructor for access control.
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
    public AccessControl(DbSet<TEntity> dataSet, bool entityHasOwnership, bool entityHasExplicitAccessControl, bool entityHasVisibility, Func<TEntity, string>? getOwnerIdFromEntity, Func<TEntity, bool>? checkIfPublic, Func<TEntity, bool>? checkIfHidden, Func<TEntity, List<string>>? getUsersWithExplicitAccess, UserManager<IdentityUser> userManager)
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

        var results = this.DataSet.Where(a => this.CanRead(a, userIdentity, userRoles));
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

    private bool CanRead(TEntity a, IdentityUser? user, string[] userRoles)
    {
        bool isHidden = false;
        if (this.EntityHasVisibility)
        {
            isHidden = this.CheckIfHidden!(a);

            bool isPublic = this.CheckIfPublic!(a);
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

        if (isHidden)
        {
            // Nobody else gets access if hidden.
            return false;
        }

        if (this.EntityHasOwnership)
        {
            var isOwner = this.GetOwnerIdFromEntity!(a) == user.Id;
            if (isOwner)
            {
                // Owner gets access regardless of anything else.
                return true;
            }
        }


        if (this.EntityHasExplicitAccessControl)
        {
            bool userHasExplicitAccess = this.GetUsersWithExplicitAccess!(a).Contains(user.Id);
            if (userHasExplicitAccess)
            {
                // User has explicit permission to access.
                return true;
            }
        }

        return false;
    }
}