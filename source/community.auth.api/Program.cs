using System.Reflection;
using community.common.Definitions;
using community.common.Enumerations;
using community.ioc;
using community.middleware.Builders;
using community.middleware.Configurations;
using community.middleware.ServiceCollectionExtensions;
using Dapper;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var files = new[]
{
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
    $"{typeof(ContactType).Assembly.GetName().Name}.xml"
};

var swaggerConfiguration = new SwaggerConfiguration
{
    Version = "v1",
    Title = $"Authentication API for {Strings.CompanyName}",
    Description =
        """
        Hosts authentication endpoints responsible for registering a user as a community administrator to enable community registration.
        Handles all multi-factor and password driven authentication, generating valid access and refresh tokens, renewing access tokens
        utilizing the user's refresh token, and handling forgot password flows, for users who choose to login via passwords.
        """,
    DocumentationFiles = files.Select(f => Path.Combine(AppContext.BaseDirectory, f)),
    ShouldDisplayAuthorization = false
};

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.WebHost.AddOrderedConfigurations();
var container = ApplicationInstaller.Install(builder.Configuration);

builder.Services.RegisterServices(container, builder.Configuration, swaggerConfiguration);

var app = builder.Build();
app.RegisterApplication(container, swaggerConfiguration);
await app.RunAsync();