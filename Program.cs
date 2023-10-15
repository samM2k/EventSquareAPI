
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using EventSquareAPI.Controllers;

namespace EventSquareAPI;

/// <summary>
/// The main Program class.
/// </summary>
public class Program
{
    /// <summary>
    /// The program entry point.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        WebApplication.CreateBuilder(args).Configure().Run();
    }
}
