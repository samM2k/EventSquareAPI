using System.Text.Json.Nodes;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// Google's Geometry object.
/// </summary>
public record GoogleGeometry(JsonObject bounds, LatLong location, string location_type, JsonObject viewport);
