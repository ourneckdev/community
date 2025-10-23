namespace community.data.entities.Search;

/// <summary>
///     Immutable search results object.
/// </summary>
/// <param name="Id">The community id returned.  Indicates a match on name</param>
/// <param name="Name">The name of the existing community.</param>
/// <param name="AddressId">The existing address id, matched on Address1, City, State, PostalCode</param>
/// <param name="ContactId">The existing phone id, exact match on value.</param>
public record CommunitySearchResults(
    Guid Id,
    string Name,
    Guid? AddressId,
    Guid? ContactId);