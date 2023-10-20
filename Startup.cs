using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

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
        app.UseCors(a => a.SetIsOriginAllowed((a) =>
        {
            return a.Contains("http://localhost:");
        }).AllowAnyHeader().AllowCredentials());
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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventSquare API", Version = "v1" });

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
                    Array.Empty<string>()
                }
            });

            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var xmlPath = Path.Combine(basePath, "EventSquareAPI.xml"); // Specify the path to your XML documentation file
            c.IncludeXmlComments(xmlPath);
        });
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString).EnableSensitiveDataLogging());

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
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        //.AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = configuration["Jwt:Issuer"],
        //        ValidAudience = configuration["Jwt:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Unable to get JWT Secret from appSettings.")))
        //    };
        //})
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromDays(30); // Adjust the expiration time as needed

            options.Cookie.SameSite = SameSiteMode.None;
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return Task.CompletedTask;
                }
            };

        });

        //builder.Services.AddScoped(provider =>
        //{
        //    var secret = configuration["Jwt:Secret"];
        //    var audience = configuration["Jwt:Audience"];
        //    var issuer = configuration["Jwt:Issuer"];

        //    if (secret is null)
        //    {
        //        throw new InvalidOperationException("Unable to get JWT Secret from Configuration.");
        //    }

        //    return new JwtTokenHandler(secret, audience, issuer);
        //});


        builder.Services.AddAuthorization();
    }
}
