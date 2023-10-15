using System.ComponentModel.DataAnnotations;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// The user login.
/// </summary>
public class LoginModel
{
    /// <summary>
    /// The user login.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="password">The user's password.</param>
    public LoginModel(string email, string password) : base()
    {
        this.Email = email;
        this.Password = password;
    }

    /// <summary>
    /// Gets or sets the user's email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [Required]
    [MinLength(6)] // Adjust the minimum password length as needed
    public string Password { get; set; }
}