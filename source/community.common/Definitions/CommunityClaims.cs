namespace community.common.Definitions;

/// <summary>
///     Defines the custom claims added to the JWT.
/// </summary>
public static class CommunityClaims
{
    /// <summary>
    ///     Which community the member is currently logged in to.
    /// </summary>
    public const string CurrentCommunityId = "cci";

    /// <summary>
    ///     What the user's ID is in the database.
    /// </summary>
    public const string UserId = "uid";
}