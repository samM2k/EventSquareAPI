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
    /// Validate a users session.
    /// </summary>
    /// <returns>A result indicating the success or failure of authentication.</returns>
    [HttpGet]
    [Route("ValidateToken")]
    [Authorize]
    public async Task<ObjectResult> ValidateToken()
    {
        var user = await this._userManager.GetUserAsync(this.HttpContext.User);

        if (user is null)
        {
            return this.Problem("User not found.");
        }
        return new OkObjectResult(user);
    }
}
