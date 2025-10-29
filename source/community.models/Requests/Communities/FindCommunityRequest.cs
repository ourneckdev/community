using community.data.entities.Search;

namespace community.models.Requests.Communities;

/// <summary>
///     Searches for an existing community using the provided information
/// </summary>
/// <param name="Name"></param>
/// <param name="AddressLine1"></param>
/// <param name="City"></param>
/// <param name="StateCode"></param>
/// <param name="PostalCode"></param>
/// <param name="PhoneNumber"></param>
public record FindCommunityRequest(
    string? Name,
    string? AddressLine1,
    string? City,
    string? StateCode,
    string? PostalCode,
    string? PhoneNumber)
{
    /// <summary>
    /// converts the request object to the expected database record used for searching
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static implicit operator FindCommunityRecord(FindCommunityRequest request) 
        => new FindCommunityRecord(request.Name, 
            request.AddressLine1, 
            request.City, 
            request.StateCode,
            request.PostalCode, 
            request.PhoneNumber);
};