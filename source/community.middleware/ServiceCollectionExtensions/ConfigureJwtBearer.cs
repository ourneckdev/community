using System.Text;
using community.common.AppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace community.middleware.ServiceCollectionExtensions;

/// <summary>
///     Adds JWT Bearer token support to the APIs
/// </summary>
public static class ConfigureJwtBearer
{
    /// <summary>
    ///     Initializes the Jwt authentication and authorization.
    /// </summary>
    /// <param name="services">The ServiceCollection that is being extended.</param>
    /// <param name="configuration">The configuration previously loaded.</param>
    /// <exception cref="InvalidOperationException">Exception thrown if the symmetric key isn't defined.</exception>
    public static void AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings?.Secret ?? throw new InvalidOperationException())),
                    RequireSignedTokens = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer
                };
            });
    }
}   