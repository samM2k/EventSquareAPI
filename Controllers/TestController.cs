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
    new Location(49548, "Road 200", "O'Neals", "CA", "United States", 37.153463, -119.648192),
    new Location(81, "Seaton Place Northwest", "Washington", "DC", "United States", 38.9149499, -77.01170259999999),
    new Location(1267, "Martin Street", "Nashville", "TN", "United States", 36.1404897, -86.7695179),
    new Location(7431, "Candace Way", "Louisville", "KY", "United States", 38.142768, -85.7717132),
    new Location(1407, "Walden Court", "Crofton", "MD", "United States", 39.019306, -76.660653),
    new Location(5906, "Milton Avenue", "Deale", "MD", "United States", 38.784451, -76.54125499999999),
    new Location(74, "Springfield Street", "Agawam", "MA", "United States", 42.0894922, -72.6297558),
    new Location(2905, "Stonebridge Court", "Norman", "OK", "United States", 35.183319, -97.40210499999999),
    new Location(20930, "Todd Valley Road", "Foresthill", "CA", "United States", 38.989466, -120.883108),
    new Location(5928, "West Mauna Loa Lane", "Glendale", "AZ", "United States", 33.6204899, -112.18702),
    new Location(802, "Madison Street Northwest", "Washington", "DC", "United States", 38.9582381, -77.0244287),
    new Location(2811, "Battery Place Northwest", "Washington", "DC", "United States", 38.9256252, -77.0982646),
    new Location(210, "Lacross Lane", "Westmore", "VT", "United States", 44.771005, -72.048664),
    new Location(2010, "Rising Hill Drive", "Norman", "OK", "United States", 35.177281, -97.411869),
    new Location(450, "Kinhawk Drive", "Nashville", "TN", "United States", 36.030927, -86.71949099999999),
    new Location(131, "Westerly Street", "Manchester", "CT", "United States", 41.7906813, -72.53559729999999),
    new Location(308, "Woodleaf Court", "Glen Burnie", "MD", "United States", 39.1425931, -76.6238441),
    new Location(8502, "Madrone Avenue", "Louisville", "KY", "United States", 38.1286407, -85.8678042),
    new Location(816, "West 19th Avenue", "Anchorage", "AK", "United States", 61.203221, -149.898655),
    new Location(172, "Alburg Springs Road", "Alburgh", "VT", "United States", 44.995827, -73.2201539),
    new Location(159, "Downey Drive", "Manchester", "CT", "United States", 41.7800126, -72.5754309),
    new Location(125, "John Street", "Santa Cruz", "CA", "United States", 36.950901, -122.046881),
    new Location(1101, "Lotus Avenue", "Glen Burnie", "MD", "United States", 39.191982, -76.6525659),
    new Location(8376, "Albacore Drive", "Pasadena", "MD", "United States", 39.110409, -76.46565799999999),
    new Location(491, "Arabian Way", "Grand Junction", "CO", "United States", 39.07548999999999, -108.474785),
    new Location(12245, "West 71st Place", "Arvada", "CO", "United States", 39.8267078, -105.1366798),
    new Location(80, "North East Street", "Holyoke", "MA", "United States", 42.2041219, -72.5977704),
    new Location(4695, "East Huntsville Road", "Fayetteville", "AR", "United States", 36.0471975, -94.0946286),
    new Location(310, "Timrod Road", "Manchester", "CT", "United States", 41.756758, -72.493501),
    new Location(1364, "Capri Drive", "Panama City", "FL", "United States", 30.2207276, -85.6808795),
    new Location(132, "Laurel Green Court", "Savannah", "GA", "United States", 32.0243075, -81.2468102),
    new Location(6657, "West Rose Garden Lane", "Glendale", "AZ", "United States", 33.676018, -112.201658),
    new Location(519, "West 75th Avenue", "Anchorage", "AK", "United States", 61.15288690000001, -149.889133),
    new Location(31353, "Santa Elena Way", "Union City", "CA", "United States", 37.593981, -122.059762),
    new Location(8398, "West Denton Lane", "Glendale", "AZ", "United States", 33.515353, -112.240812),
    new Location(700, "Winston Place", "Anchorage", "AK", "United States", 61.215882, -149.737337),
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
