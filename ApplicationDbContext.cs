using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventSquareAPI;

/// <summary>
/// The data context for the application.
/// </summary>
/// <remarks>Inherits ASP.NET Identity's IdentityDbContext</remarks>
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
    }
}