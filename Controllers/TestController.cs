using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventSquareAPI.Controllers;

/// <summary>
/// A test controller for validating tokens.
/// </summary>
public class TestController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    /// <summary>
    /// Test controller.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="context">The data context.</param>
    public TestController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        this._userManager = userManager;
        this._context = context;
    }

    /// <summary>
    /// Validate a users session.
    /// </summary>
    /// <returns>A result indicating the success or failure of authentication.</returns>
    [HttpGet]
    [Route("api/Validate")]
    [Authorize]
    public async Task<ObjectResult> ValidateToken()
    {
        var user = await this._userManager.GetUserAsync(this.HttpContext.User);

        if (user is null)
        {
            return this.Problem("User not found.");
        }
        return new OkObjectResult(user);
    }

    /// <summary>
    /// Generate test data.
    /// </summary>
    /// <returns>The HTTP Response.</returns>
    [HttpGet]
    [Route("api/generateTestData")]
    public async Task<IActionResult> GenerateTestData()
    {
        Random rnd = new Random();

        var locations = new Location[] {
    new Location(1, "123 Main Street", "New York", "New York", "USA"),
    new Location(2, "456 Elm Avenue", "Los Angeles", "California", "USA"),
    new Location(3, "789 Oak Lane", "Chicago", "Illinois", "USA"),
    new Location(4, "101 Pine Road", "London", "England", "UK"),
    new Location(5, "202 Maple Drive", "Paris", "Ile-de-France", "France"),
    new Location(6, "303 Cedar Street", "Sydney", "New South Wales", "Australia"),
    new Location(7, "404 Birch Avenue", "Toronto", "Ontario", "Canada"),
    new Location(8, "505 Redwood Court", "Berlin", "Berlin", "Germany"),
    new Location(9, "606 Spruce Road", "Tokyo", "Tokyo", "Japan"),
    new Location(10, "707 Willow Lane", "Dubai", "Dubai", "UAE"),
};


        var names = new string[] {
    "Summer Solstice Festival",
    "Midnight Masquerade Ball",
    "Epic Movie Marathon",
    "Pancake Breakfast Fundraiser",
    "Science Fiction Convention",
    "Beach Bonfire Bash",
    "Charity Poker Night",
    "Art Gallery Opening",
    "Carnival Extravaganza",
    "Hiking Adventure Weekend",
    "Cosplay Contest",
    "Karaoke Night Out",
    "Gourmet Food Tasting",
    "Tech Startup Launch Party",
    "Dog Adoption Fair",
    "Yoga and Wellness Retreat",
    "Superhero Costume Party",
    "Dance Dance Revolution Showdown",
    "Vintage Car Show",
    "Zombie Apocalypse Survival Training",
};

        var descriptions = new string[] {
    "Join us for a magical evening under the stars at the Summer Solstice Festival. Live music, food trucks, and dancing await you.",
    "Put on your best mask and dance the night away at the Midnight Masquerade Ball. A night of mystery and elegance.",
    "Get your popcorn ready for an epic movie marathon featuring the greatest classics of all time. It's a film lover's dream come true.",
    "Start your day with a stack of delicious pancakes at our breakfast fundraiser. All proceeds go to a good cause.",
    "Embrace your inner geek at the Science Fiction Convention. Meet your favorite authors and immerse yourself in the world of sci-fi.",
    "S'mores, bonfires, and beach vibes! Join us for a Beach Bonfire Bash, a perfect summer night by the sea.",
    "Test your poker face at our Charity Poker Night. Prizes, fun, and all for a charitable cause.",
    "Step into the world of art at the Art Gallery Opening. Explore stunning works and mingle with fellow art enthusiasts.",
    "Cotton candy, rides, and games! The Carnival Extravaganza is coming to town for a weekend of family fun.",
    "Embark on a thrilling hiking adventure weekend. Conquer the trails and enjoy the beauty of the great outdoors.",
    "Cosplayers unite at the Cosplay Contest. Show off your creativity and compete for fantastic prizes.",
    "Sing your heart out at Karaoke Night Out. Belt your favorite tunes and enjoy a night of musical fun.",
    "Indulge in a gourmet food tasting experience. Discover new flavors and savor exquisite dishes.",
    "Celebrate the launch of the next big tech startup at our Launch Party. Be the first to see what's in store.",
    "Find your furry friend at the Dog Adoption Fair. Give a loving home to a dog in need.",
    "Recharge and rejuvenate at the Yoga and Wellness Retreat. It's time to prioritize self-care.",
    "Dress up as your favorite superhero and join our Costume Party. It's a night of capes and heroics.",
    "Compete in a Dance Dance Revolution Showdown. Show off your dance moves and claim the title of DDR champion.",
    "Admire the beauty of vintage cars at the Vintage Car Show. Classic automobiles from a bygone era on display.",
    "Prepare for the zombie apocalypse with Survival Training. Learn essential skills to survive the undead.",
};

        List<CalendarEvent> sampleEvents = new();
        for (int i = 0; i < 25; i++)
        {
            DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(rnd.Next(182)).AddMinutes(rnd.Next(0, 180));
            DateTimeOffset end = start.AddHours(rnd.Next(1, 6));
            Location location = locations[rnd.Next(locations.Length - 1)];
            var name = names[rnd.Next(names.Length - 1)];
            var description = descriptions[rnd.Next(descriptions.Length - 1)];
            bool isVirtual = rnd.Next(0, 1) == 1;
            bool isPhysical = !isVirtual || rnd.Next(0, 1) == 1;
            string owner = Guid.NewGuid().ToString();

            var randomEvent = new CalendarEvent(start, end, name, description, isVirtual, isPhysical, owner, location);

            randomEvent.Visibility = EventVisibility.Public;

            sampleEvents.Add(randomEvent);

        }

        this._context.Events.AddRange(sampleEvents);
        try
        {
            await this._context.SaveChangesAsync();
            return this.Ok(sampleEvents);
        }
        catch (Exception ex)
        {
            return this.Problem(ex.Message);
        }
    }
}
