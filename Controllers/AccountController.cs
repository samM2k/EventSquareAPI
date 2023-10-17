using System.Security.Claims;

using EventSquareAPI.DataTypes;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventSquareAPI.Controllers;


/// <summary>
/// Account management controller.
/// </summary>
[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    //private readonly JwtTokenHandler _tokenHandler;

    /// <summary>
    /// The account controller.
    /// </summary>
    /// <param name="userManager">The ASP.NET Identity UserManager.</param>
    //// <param name="tokenHandler">The Token Handler.</param>
    public AccountController(UserManager<IdentityUser> userManager)
    {
        this._userManager = userManager;
        //this._tokenHandler = tokenHandler;
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="login">The login credentials.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] LoginModel login)
    {
        if (this.ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = login.Email,
                Email = login.Email,
                // You can add additional user properties here
            };

            var result = await this._userManager.CreateAsync(user, login.Password);

            if (result.Succeeded)
            {
                // User registration successful, you may also generate a token and send it as a response if needed
                return this.Ok(new { Message = "User registered successfully" });
            }

            // If registration fails, return the error messages
            return this.BadRequest(new { result.Errors });
        }

        return this.BadRequest(new { Message = "Invalid login" });
    }

    /// <summary>
    /// Exchange user login credentials for an access token.
    /// </summary>
    /// <param name="login">The user login credentials.</param>
    /// <returns>An access token.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        // Validate the user's credentials (e.g., using UserManager and SignInManager)
        IdentityUser? user = await this._userManager.FindByEmailAsync(login.Email);

        if (user == null || !await this._userManager.CheckPasswordAsync(user, login.Password))
        {
            return this.Unauthorized("Invalid username or password."); // Invalid credentials
        }

        // Create the claims for the user (You can customize this based on your requirements)
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name.ToString(), user.UserName?? string.Empty),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
    };

        // Create a Claims Identity with the claims
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Sign in the user with a cookie
        await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        // Generate a JWT token for the user (if needed)
        //var token = this._tokenHandler.GetToken(user);

        return this.NoContent();
    }
}
