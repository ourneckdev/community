namespace community.common.Definitions;

/// <summary>
///     Defines the types of users available in the system.
/// </summary>
public static class UserTypes
{
    /// <summary>
    ///     Maps the database guids to a string for display.
    /// </summary>
    public static Dictionary<Guid, string> Values => new()
    {
        { Guid.Parse("0194e2a0-1dd9-7d58-bdb6-f8eae6732cc2"), Strings.UserType_SiteAdministrator },
        { Guid.Parse("0194e2a0-1ddb-7ced-92ec-6a2ee36ed7db"), Strings.UserType_SupportAdministrator },
        { Guid.Parse("0194e2a0-1ddb-7d70-aa17-4087a157d6a2"), Strings.UserType_CommunityAdministrator },
        { Guid.Parse("0194e2a0-1ddb-7da9-bf6e-c9fea6787578"), Strings.UserType_CommunityMember }
    };

    /// <summary>
    /// Returns the key for a supplied string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid GetKey(string value)
    {
        return Values.FirstOrDefault(v => v.Value == value).Key;
    }
}