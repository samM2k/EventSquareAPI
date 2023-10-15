using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace EventSquareAPI;

/// <summary>
/// Handles the startup logic.
/// </summary>
public static class Startup
{
    /// <summary>
    /// Configures the web application.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application</returns>
    public static WebApplication Configure(this WebApplicationBuilder builder)
    {
        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureMiddleware(app);

        return app;
    }

    /// <summary>
    /// Configures the middleware for the app to use.
    /// </summary>
    /// <param name="app">The web application.</param>
    private static void ConfigureMiddleware(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
    }

    /// <summary>
    /// Configures the necessary services for the application.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });

            // Add security definitions and requirements for Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            var basePath =  PlatformServices.Default.Application.ApplicationBasePath;
            var xmlPath = Path.Combine(basePath, "EventSquareAPI.xml"); // Specify the path to your XML documentation file
            c.IncludeXmlComments(xmlPath);
        });
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));

        ConfigureIdentity(builder);
    }

    /// <summary>
    /// Configures Identity, Authentication, and Authorization.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    private static void ConfigureIdentity(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


        builder.Services.Configure<IdentityOptions>(options =>
        {

            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException("Unable to get JWT Secret from appSettings.")))
            };
        });

        builder.Services.AddAuthorization();
    }
}
