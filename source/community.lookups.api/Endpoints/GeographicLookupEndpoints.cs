using Carter;
using community.providers.lookups.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace community.lookups.api.Endpoints;

/// <summary>
/// Defines endpoints for geogrphical lookup lists.
/// </summary>
public class GeographicLookupEndpoints : ICarterModule
{
    /// <summary>
    /// registers endpoints related to retrieving locale lookup data.
    /// </summary>
    /// <param name="app"></param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/countries", async ([FromServices] ILocaleProvider localeProvider)
            => Results.Ok(await localeProvider.ListCountriesAsync()));
        
        app.MapGet("/countries/{countryCode}/states/", 
            async ([FromServices] ILocaleProvider localeProvider, string countryCode) 
                => Results.Ok(await localeProvider.ListStatesAsync(countryCode)));
        
        app.MapGet("/countries/{countryCode}/states/{stateCode}/counties/", 
            async ([FromServices] ILocaleProvider localeProvider, string countryCode, string stateCode)
                => Results.Ok(await localeProvider.ListCountiesAsync(countryCode, stateCode)));
    }
}