using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSquareAPI.Controllers;

/// <summary>
/// A test controller for validating tokens.
/// </summary>
public class TestController : ControllerBase
{
    public TestController()
    {}

    /// <summary>
    /// Validates a users session.
    /// </summary>
    /// <returns>A result indicating the success or failure of authentication.</returns>
    [HttpGet]
    [Route("ValidateToken")]
    [Authorize]
    public StatusCodeResult ValidateToken()
    {
        StatusCodeResult result = new(200);
        return new(200);
    }
}
