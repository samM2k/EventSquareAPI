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
    /// <param name = "userManager" > The user manager.</param>
    /// <param name = "context" > The data context.</param>
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
    new Location("49548 Road 200", null, 49548, "Road 200", "O'Neals", "CA", 93645, "United States", new(37.153463, -119.648192)),
    new Location("81 Seaton Place Northwest", null, 81, "Seaton Place Northwest", "Washington", "DC", 20001, "United States", new(38.9149499, -77.01170259999999)),
    new Location("1267 Martin Street", 203, 1267, "Martin Street", "Nashville", "TN", 37203, "United States", new(36.1404897, -86.7695179)),
    new Location("7431 Candace Way", 1, 7431, "Candace Way", "Louisville", "KY", 40214, "United States", new(38.142768, -85.7717132)),
    new Location("1407 Walden Court", null, 1407, "Walden Court", "Crofton", "MD", 21114, "United States", new(39.019306, -76.660653)),
    new Location("5906 Milton Avenue", null, 5906, "Milton Avenue", "Deale", "MD", 20751, "United States", new(38.784451, -76.54125499999999)),
    new Location("74 Springfield Street", null, 74, "Springfield Street", "Agawam", "MA", 01001, "United States", new(42.0894922, -72.6297558)),
    new Location("2905 Stonebridge Court", null, 2905, "Stonebridge Court", "Norman", "OK", 73071, "United States", new(35.183319, -97.40210499999999)),
    new Location("20930 Todd Valley Road", null, 20930, "Todd Valley Road", "Foresthill", "CA", 95631, "United States", new(38.989466, -120.883108)),
    new Location("5928 West Mauna Loa Lane", null, 5928, "West Mauna Loa Lane", "Glendale", "AZ", 85306, "United States", new(33.6204899, -112.18702)),
    new Location("802 Madison Street Northwest", null, 802, "Madison Street Northwest", "Washington", "DC", 20011, "United States", new(38.9582381, -77.0244287)),
    new Location("2811 Battery Place Northwest", null, 2811, "Battery Place Northwest", "Washington", "DC", 20016, "United States", new(38.9256252, -77.0982646)),
    new Location("210 Lacross Lane", null, 210, "Lacross Lane", "Westmore", "VT", 05860, "United States", new(44.771005, -72.048664)),
    new Location("2010 Rising Hill Drive", null, 2010, "Rising Hill Drive", "Norman", "OK", 73071, "United States", new(35.177281, -97.411869)),
    new Location("450 Kinhawk Drive", null, 450, "Kinhawk Drive", "Nashville", "TN", 37211, "United States", new(36.030927, -86.71949099999999)),
    new Location("131 Westerly Street", null, 131, "Westerly Street", "Manchester", "CT", 06042, "United States", new(41.7906813, -72.53559729999999)),
    new Location("308 Woodleaf Court", null, 308, "Woodleaf Court", "Glen Burnie", "MD", 21061, "United States", new(39.1425931, -76.6238441)),
    new Location("8502 Madrone Avenue", null, 8502, "Madrone Avenue", "Louisville", "KY", 40258, "United States", new(38.1286407, -85.8678042)),
    new Location("816 West 19th Avenue", null, 816, "West 19th Avenue", "Anchorage", "AK", 99503, "United States", new(61.203221, -149.898655)),
    new Location("172 Alburg Springs Road", null, 172, "Alburg Springs Road", "Alburgh", "VT", 05440, "United States", new(44.995827, -73.2201539)),
    new Location("159 Downey Drive", null, 159, "Downey Drive", "Manchester", "CT", 06040, "United States", new(41.7800126, -72.5754309)),
    new Location("125 John Street", null, 125, "John Street", "Santa Cruz", "CA", 95060, "United States", new(36.950901, -122.046881)),
    new Location("1101 Lotus Avenue", null, 1101, "Lotus Avenue", "Glen Burnie", "MD", 21061, "United States", new(39.191982, -76.6525659)),
    new Location("8376 Albacore Drive", null, 8376, "Albacore Drive", "Pasadena", "MD", 21122, "United States", new(39.110409, -76.46565799999999)),
    new Location("491 Arabian Way", null, 491, "Arabian Way", "Grand Junction", "CO", 81504, "United States", new(39.07548999999999, -108.474785)),
    new Location("12245 West 71st Place", null, 12245, "West 71st Place", "Arvada", "CO", 80004, "United States", new(39.8267078, -105.1366798)),
    new Location("80 North East Street", 4, 80, "North East Street", "Holyoke", "MA", 01040, "United States", new(42.2041219, -72.5977704)),
    new Location("4695 East Huntsville Road", null, 4695, "East Huntsville Road", "Fayetteville", "AR", 72701, "United States", new(36.0471975, -94.0946286)),
    new Location("310 Timrod Road", null, 310, "Timrod Road", "Manchester", "CT", 06040, "United States", new(41.756758, -72.493501)),
    new Location("1364 Capri Drive", null, 1364, "Capri Drive", "Panama City", "FL", 32405, "United States", new(30.2207276, -85.6808795)),
    new Location("132 Laurel Green Court", null, 132, "Laurel Green Court", "Savannah", "GA", 31419, "United States", new(32.0243075, -81.2468102)),
    new Location("6657 West Rose Garden Lane", null, 6657, "West Rose Garden Lane", "Glendale", "AZ", 85308, "United States", new(33.676018, -112.201658)),
    new Location("519 West 75th Avenue", 000003, 519, "West 75th Avenue", "Anchorage", "AK", 99518, "United States", new(61.15288690000001, -149.889133)),
    new Location("31353 Santa Elena Way", null, 31353, "Santa Elena Way", "Union City", "CA", 94587, "United States", new(37.593981, -122.059762)),
    new Location("8398 West Denton Lane", null, 8398, "Denton Lane", "Glendale", "AZ", 85305, "United States", new(33.515353, -112.240812)),
    new Location("700 Winston Place", null, 700, "Winston Place", "Anchorage", "AK", 99504, "United States", new(61.215882, -149.737337)),
    new Location("232 Maine Avenue", null, 232, "Maine Avenue", "Panama City", "FL", 32401, "United States", new(30.1527033, -85.63207129999999)),
    new Location("1 Kempf Drive", null, 1, "Kempf Drive", "Easton", "MA", 02375, "United States", new(42.0505989, -71.08029379999999)),
    new Location("5811 Crossings Boulevard", null, 5811, "Crossings Boulevard", "Nashville", "TN", 37013, "United States", new(36.0370847, -86.6413728)),
    new Location("5108 Franklin Street", null, 5108, "Franklin Street", "Savannah", "GA", 31405, "United States", new(32.034987, -81.121928)),
    new Location("913 Fallview Trail", null, 913, "Fallview Trail", "Nashville", "TN", 37211, "United States", new(36.02419100000001, -86.718305)),
    new Location("270 Chrissy's Court", null, 270, "Chrissy's Court", "Westmore", "VT", 05443, "United States", new(44.1710043, -73.1065617)),
    new Location("130 Old Route 103", null, 130, "Old Route 103", "Chester", "VT", 05143, "United States", new(43.224335, -72.54227399999999)),
    new Location("10826 Pointe Royal Drive", null, 10826, "Pointe Royal Drive", "Bakersfield", "CA", 93311, "United States", new(35.2930007, -119.1225908)),
    new Location("74 Ranch Drive", null, 74, "Ranch Drive", "Montgomery", "AL", 36109, "United States", new(32.383322, -86.235124)),
    new Location("6601 West Ocotillo Road", null, 6601, "West Ocotillo Road", "Glendale", "AZ", 85301, "United States", new(33.53433, -112.2011246)),
    new Location("19416 Barclay Road", null, 19416, "Barclay Road", "Castro Valley", "CA", 94546, "United States", new(37.70382, -122.091054)),
    new Location("1347 Blackwalnut Court", null, 1347, "Blackwalnut Court", "Annapolis", "MD", 21403, "United States", new(38.936881, -76.475823)),
    new Location("1770 Colony Way", null, 1770, "Colony Way", "Fayetteville", "AR", 72704, "United States", new(36.0867, -94.229754)),
    new Location("165 Saint John Street", null, 165, "Saint John Street", "Manchester", "CT", 06040, "United States", new(41.7762171, -72.5410548)),
    new Location("2409 Research Boulevard", null, 2409, "Research Boulevard", "Fort Collins", "CO", 80526, "United States", new(40.554586, -105.087852)),
    new Location("1903 Bashford Manor Lane", null, 1903, "Bashford Manor Lane", "Louisville", "KY", 40218, "United States", new(38.1977059, -85.675288)),
    new Location("8315 Surf Drive", null, 8315, "Surf Drive", "Panama City Beach", "FL", 32408, "United States", new(30.163458, -85.785449)),
    new Location("3301 Old Muldoon Road", null, 3301, "Old Muldoon Road", "Anchorage", "AK", 99504, "United States", new(61.1908348, -149.7340096)),
    new Location("8800 Cordell Circle", 000003, 8800, "Cordell Circle", "Anchorage", "AK", 99502, "United States", new(61.1409305, -149.9437822)),
    new Location("117 East Cook Avenue", null, 117, "East Cook Avenue", "Anchorage", "AK", 99501, "United States", new(61.230336, -149.883795)),
    new Location("6231 North 67th Avenue", 241, 6231, "North 67th Avenue", "Glendale", "AZ", 85301, "United States", new(33.5279666, -112.2022551)),
    new Location("8505 Waters Avenue", 66, 8505, "Waters Avenue", "Savannah", "GA", 31406, "United States", new(31.9901877, -81.1070672)),
    new Location("7 Underwood Place Northwest", null, 7, "Underwood Place", "Washington", "DC", 20012, "United States", new(38.969351, -77.009722)),
    new Location("21950 Arnold Center Road", null, 21950, "Arnold Center Road", "Carson", "CA", 90810, "United States", new(33.8272706, -118.2302826)),
    new Location("1427 South Carolina Avenue Southeast", null, 1427, "South Carolina Avenue", "Washington", "DC", 20003, "United States", new(38.886615, -76.9845349)),
    new Location("1420 Turtleback Trail", null, 1420, "Turtleback Trail", "Panama City", "FL", 32413, "United States", new(30.281084, -85.9677169)),
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
