using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EventSquareAPI;

/// <summary>
/// The data context for the application.
/// </summary>
/// <remarks>Inherits ASP.NET Identity's IdentityDbContext</remarks>
public class ApplicationDbContext : IdentityDbContext
{
    /// <summary>
    /// The data context for the application.
    /// </summary>
    /// <param name="options">The options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
    }


    /// <summary>
    /// Gets or sets the calendar events.
    /// </summary>
    public DbSet<CalendarEvent> Events { get; set; }

    /// <summary>
    /// Gets or sets the RSVPs.
    /// </summary>
    public DbSet<Rsvp> Rsvps { get; set; }

    /// <summary>
    /// Invoked on creation of Entity Relationship Model.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CalendarEvent>().OwnsOne(a=>a.Location);
        builder.Entity<Rsvp>().HasOne(a => a.Event).WithMany(a=>a.Rsvps).HasForeignKey(a=>a.EventId);
    }

}