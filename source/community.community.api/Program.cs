using System.Reflection;
using community.common.Definitions;
using community.common.Enumerations;
using community.ioc;
using community.middleware.Builders;
using community.middleware.Configurations;
using community.middleware.ServiceCollectionExtensions;
using Dapper;

DefaultTypeMap.MatchNamesWithUnderscores = true;
// SqlMapper.AddTypeHandler(typeof(ContactType), new TypeHandler<ContactType>());
// SqlMapper.AddTypeHandler(typeof(EntityType), new TypeHandler<EntityType>());

var files = new[]
{
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
    $"{typeof(ContactType).Assembly.GetName().Name}.xml"
};

var swaggerConfiguration = new SwaggerConfiguration
{
    Version = "v1",
    Title = $"Community API for {Strings.CompanyName}",
    Description = "Endpoints for interacting with the community and it's users.",
    DocumentationFiles = files.Select(f => Path.Combine(AppContext.BaseDirectory, f))
};

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.WebHost.AddOrderedConfigurations();
var container = ApplicationInstaller.Install(builder.Configuration);

builder.Services.RegisterServices(container, builder.Configuration, swaggerConfiguration);

var app = builder.Build();
app.RegisterApplication(container, swaggerConfiguration);
await app.RunAsync();