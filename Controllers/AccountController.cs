using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventSquareAPI.DataTypes;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="configuration"></param>
    public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="login">The login credentials.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] LoginModel login)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = login.Email,
                Email = login.Email,
                // You can add additional user properties here
            };

            var result = await _userManager.CreateAsync(user, login.Password);

            if (result.Succeeded)
            {
                // User registration successful, you may also generate a token and send it as a response if needed
                return Ok(new { Message = "User registered successfully" });
            }

            // If registration fails, return the error messages
            return BadRequest(new { result.Errors });
        }

        return BadRequest(new { Message = "Invalid login" });
    }

    /// <summary>
    /// Exchange user login credentials for an access token.
    /// </summary>
    /// <param name="login">The user login credentials.</param>
    /// <returns>An access token.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        if (ModelState.IsValid)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(login.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id ),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty )
                    // You can add more claims as needed
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30), // You can adjust the expiration time
                    signingCredentials: creds
                );

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized(new { Message = "Invalid email or password" });
        }

        return BadRequest(new { Message = "Invalid model" });
    }
}
