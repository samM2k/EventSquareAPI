using System.ComponentModel.DataAnnotations;

namespace EventSquareAPI;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)] // Adjust the minimum password length as needed
    public string Password { get; set; }
}