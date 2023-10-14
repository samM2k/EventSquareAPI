using System.ComponentModel.DataAnnotations;

namespace EventSquareAPI.DataTypes;

/// <summary>
/// The user login.
/// </summary>
public class LoginModel
{
    public LoginModel(string email, string password) : base()
    {
        Email = email;
        Password = password;
    }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)] // Adjust the minimum password length as needed
    public string Password { get; set; }
}