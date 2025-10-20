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
    Title = $"Configuration Lookup API for {Strings.CompanyName}",
    Description = "Hosts endpoints for retrieving lookup values.",
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