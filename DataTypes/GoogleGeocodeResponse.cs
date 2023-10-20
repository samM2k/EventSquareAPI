namespace EventSquareAPI.DataTypes;

/// <summary>
/// Response data from google geocoding request.
/// </summary>
public record GoogleGeocodeResponse(GoogleLocation[] results, string status);
