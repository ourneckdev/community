// ReSharper disable InconsistentNaming

namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
///     Represents the various statuses that can be returned from the GoogleSettings Geocoding API Call
/// </summary>
public enum Status
{
    /// <summary>
    ///     indicates that no errors occurred; the address was successfully parsed and at least one geocode was returned.
    /// </summary>
    OK,

    /// <summary>
    ///     indicates that the geocode was successful but returned no results. This may occur if the geocoder was passed a
    ///     non-existent address.
    /// </summary>
    ZERO_RESULTS,

    /// <summary>
    ///     indicates any of the following:
    ///     The API key is missing or invalid.
    ///     Billing has not been enabled on your account.
    ///     A self-imposed usage cap has been exceeded.
    ///     The provided method of payment is no longer valid (for example, a credit card has expired).
    /// </summary>
    OVER_DAILY_LIMIT,

    /// <summary>
    ///     ndicates that you are over your quota.
    /// </summary>
    OVER_QUERY_LIMIT,

    /// <summary>
    ///     indicates that your request was denied.
    /// </summary>
    REQUEST_DENIED,

    /// <summary>
    ///     generally indicates that the query (address, components or latlng) is missing.
    /// </summary>
    INVALID_REQUEST,

    /// <summary>
    ///     indicates that the request could not be processed due to a server error. The request may succeed if you try again.
    /// </summary>
    UNKNOWN_ERROR
}

/// <summary>
///     list of travel modes that the navigation point is not accessible from
/// </summary>
public enum RestrictedTravelMode
{
    /// <summary>
    ///     the travel mode corresponding to driving directions.
    /// </summary>
    DRIVE,

    /// <summary>
    ///     the travel mode corresponding to walking directions.
    /// </summary>
    WALK
}