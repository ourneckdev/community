namespace community.data.entities.Search;

/// <summary>
///     Search for an existing community using supplied properties
/// </summary>
/// <param name="Name">The name of the community to attepmt to match</param>
/// <param name="Addressline1"></param>
/// <param name="City"></param>
/// <param name="StateCode"></param>
/// <param name="PostalCode"></param>
/// <param name="PhoneNumber"></param>
public record FindCommunityRecord(
    string Name,
    string? Addressline1,
    string? City,
    string? StateCode,
    string? PostalCode,
    string? PhoneNumber);