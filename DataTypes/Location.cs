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
    public Location(int? flatNumber, int streetNumber, string streetName, string locality, string stateRegion, string country)
    {
        FlatNumber = flatNumber;
        StreetNumber = streetNumber;
        StreetName = streetName;
        Locality = locality;
        StateRegion = stateRegion;
        Country = country;
    }


    /// <summary>
    /// A location.
    /// </summary>
    /// <param name="streetNumber">The street number.</param>
    /// <param name="streetName">The name of the stret, including the street type.</param>
    /// <param name="locality">The suburb/town/city.</param>
    /// <param name="stateRegion">The state/region.</param>
    /// <param name="country">The country.</param>
    public Location(int streetNumber, string streetName, string locality, string stateRegion, string country) :
        this(null, streetNumber, streetName, locality, stateRegion, country)
    { }

    /// <summary>
    /// Gets or sets the flat number of the event.
    /// </summary>
    public int? FlatNumber { get; set; }

    /// <summary>
    /// Gets or sets the street number of the event.
    /// </summary>
    public int StreetNumber { get; set; }

    /// <summary>
    /// Gets or sets the street name.
    /// </summary>
    public string StreetName { get; set; }

    /// <summary>
    /// Gets or sets the locality (city/suburb) of the event.
    /// </summary>
    public string Locality { get; set; }

    /// <summary>
    /// Gets or sets the State/Region of the event.
    /// </summary>
    public string StateRegion { get; set; }

    /// <summary>
    /// Gets or sets the Country in which the event is occurring..
    /// </summary>
    public string Country { get; set; }
}