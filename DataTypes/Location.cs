namespace EventSquareAPI.DataTypes;

/// <summary>
/// An address or location.
/// </summary>
public class Location
{
    /// <summary>
    /// Gets or sets the name of the site or venue at this location.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the flat number.
    /// </summary>
    public int? FlatNumber { get; set; }

    /// <summary>
    /// Gets or sets the street number.
    /// </summary>
    public int StreetNumber { get; set; }

    /// <summary>
    /// Gets or sets the street name.
    /// </summary>
    public string StreetName { get; set; }

    /// <summary>
    /// Gets or sets the locality.
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Gets or sets the state/region.
    /// </summary>
    public string StateRegion { get; set; }

    /// <summary>
    /// Gets or sets the postcode.
    /// </summary>
    public int Postcode { get; set; }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the coordinates of the location.
    /// </summary>
    public LocationCoordinates? Coordinates { get; set; }


    /// <summary>
    /// Constructs a new location.
    /// </summary>
    /// <param name="name">The name of the site/building.</param>
    /// <param name="flatNumber">The flat number.</param>
    /// <param name="streetNumber">The street numnber.</param>
    /// <param name="streetName">The street name.</param>
    /// <param name="locality">The locality.</param>
    /// <param name="stateRegion">The state/region.</param>
    /// <param name="postcode">The postcode.</param>
    /// <param name="country">The country.</param>
    /// <param name="coordinates">The coordinates.</param>
    public Location(string? name, int? flatNumber, int streetNumber, string streetName, string locality, string stateRegion, int postcode, string country, LocationCoordinates? coordinates)
    {
        this.Name = name;
        this.FlatNumber = flatNumber;
        this.StreetNumber = streetNumber;
        this.StreetName = streetName;
        this.Locality = locality;
        this.StateRegion = stateRegion;
        this.Postcode = postcode;
        this.Country = country;
        this.Coordinates = coordinates;
    }

#pragma warning disable CS8618 // For EFCore migrations.
    private Location() { }
#pragma warning restore CS8618 
}

