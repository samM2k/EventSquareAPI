namespace EventSquareAPI.DataTypes;

/// <summary>
/// The login response object.
/// </summary>
/// <param name="Id">The user Id</param>
/// <param name="Username">The username.</param>
/// <param name="Email">The email of the user</param>
/// <param name="Phone">The phone number of the user</param>
public record LoginResponse(string? Id, string? Username, string? Email, string? Phone);
