using System.IdentityModel.Tokens.Jwt;

using EventSquareAPI.DataTypes;
using EventSquareAPI.Security;

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
    private readonly IConfiguration _configuration;
    private readonly JwtTokenHandler _tokenHandler;

    /// <summary>
    /// The account controller.
    /// </summary>
    /// <param name="userManager">The ASP.NET Identity UserManager.</param>
    /// <param name="configuration">The Configuration object.</param>
    /// <param name="tokenHandler">The Token Handler.</param>
    public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, JwtTokenHandler tokenHandler)
    {
        this._userManager = userManager;
        this._configuration = configuration;
        this._tokenHandler = tokenHandler;
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
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        if (this.ModelState.IsValid)
        {
            IdentityUser? user = await this._userManager.FindByNameAsync(login.Email);

            if (user != null && await this._userManager.CheckPasswordAsync(user, login.Password))
            {
                JwtSecurityToken token = this._tokenHandler.GetToken(user);
                return this.Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }

            return this.Unauthorized(new { Message = "Invalid email or password" });
        }

        return this.BadRequest(new { Message = "Invalid model" });
    }
}
