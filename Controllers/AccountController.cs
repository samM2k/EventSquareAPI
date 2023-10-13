using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSquareAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    // You can add additional user properties here
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // User registration successful, you may also generate a token and send it as a response if needed
                    return Ok(new { Message = "User registered successfully" });
                }

                // If registration fails, return the error messages
                return BadRequest(new { Errors = result.Errors });
            }

            return BadRequest(new { Message = "Invalid model" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id ),
                        new Claim(ClaimTypes.Name, user.UserName )
                        // You can add more claims as needed
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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
}
