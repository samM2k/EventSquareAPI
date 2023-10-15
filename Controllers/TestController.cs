using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventSquareAPI.Controllers;

/// <summary>
/// A test controller for validating tokens.
/// </summary>
public class TestController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Test controller.
    /// </summary>
    /// <param name="userManager"></param>
    public TestController(UserManager<IdentityUser> userManager)
    {
        this._userManager = userManager;
    }

    /// <summary>
    /// Validates a users session.
    /// </summary>
    /// <returns>A result indicating the success or failure of authentication.</returns>
    [HttpGet]
    [Route("ValidateToken")]
    [Authorize]
    public async Task<ObjectResult> ValidateToken()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (user is null)
        {
            return Problem("User not found.");
        }
        var roles = await _userManager.GetRolesAsync(user);
        return new OkObjectResult(user);
    }
}
