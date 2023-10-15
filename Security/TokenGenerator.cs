using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EventSquareAPI.Security;

/// <summary>
/// Handles token generation.
/// </summary>
public class TokenGenerator
{
    /// <summary>
    /// Constructs a token generator.
    /// </summary>
    /// <param name="audience"></param>
    /// <param name="issuer"></param>
    /// <param name="secret"></param>
    public TokenGenerator(string secret, string? audience, string? issuer)
    {
        this.Secret = secret;
        this.Audience = audience;
        this.Issuer = issuer;
        this.Encoding = Encoding.UTF8;
        this.SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    }

    private Encoding Encoding { get; init; }
    private string SecurityAlgorithm { get; init; }
    private string Secret { get; init; }
    private string? Audience { get; init; }
    private string? Issuer { get; init; }

    /// <summary>
    /// Gets an auth token for the given user.
    /// </summary>
    /// <param name="user">The user to generate an auth token for.</param>
    /// <returns>A Jwt Security Token for the relevant user.</returns>
    public JwtSecurityToken GetToken(IdentityUser user)
    {
        var claims = new[]
               {
                    new Claim(ClaimTypes.NameIdentifier, user.Id ),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty )
                    // You can add more claims as needed
                };

        var key = new SymmetricSecurityKey(this.Encoding.GetBytes(this.Secret ?? string.Empty));
        var creds = new SigningCredentials(key, this.SecurityAlgorithm);

        var token = new JwtSecurityToken(
            this.Issuer,
            this.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(30), // You can adjust the expiration time
            signingCredentials: creds
        );
        return token;
    }
}
