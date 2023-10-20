namespace EventSquareAPI.DataTypes;

/// <summary>
/// Googles location format.
/// </summary>
public record GoogleLocation(GoogleAddressComponent[] address_components, string formatted_address, GoogleGeometry geometry, string place_id, string[] types);
