using System.Text.Json.Serialization;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// An address or location.
/// </summary>
public class Location
{
    /// <summary>
    /// Constructs a location.
    /// </summary>
    [JsonConstructor]
    public Location(int? flatNumber, int streetNumber, string streetName, string locality, string stateRegion, string country, double? latitude, double? longitude)
    {
        this.FlatNumber = flatNumber;
        this.StreetNumber = streetNumber;
        this.StreetName = streetName;
        this.Locality = locality;
        this.StateRegion = stateRegion;
        this.Country = country;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }


    /// <summary>
    /// A location.
    /// </summary>
    /// <param name="streetNumber">The street number.</param>
    /// <param name="streetName">The name of the stret, including the street type.</param>
    /// <param name="locality">The suburb/town/city.</param>
    /// <param name="stateRegion">The state/region.</param>
    /// <param name="country">The country.</param>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    public Location(int streetNumber, string streetName, string locality, string stateRegion, string country, double? latitude, double? longitude) :
        this(null, streetNumber, streetName, locality, stateRegion, country, latitude, longitude)
    { }

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
    /// Gets or sets the locality (city/suburb).
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Gets or sets the State/Region.
    /// </summary>
    public string StateRegion { get; set; }

    /// <summary>
    /// Gets or sets the Country..
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the longitude for the location.
    /// </summary>
    public double? Latitude { get; set; }


    /// <summary>
    /// Gets or sets the longitude for the location.
    /// </summary>
    public double? Longitude { get; set; }
}