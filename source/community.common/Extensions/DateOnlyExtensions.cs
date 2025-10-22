using community.common.Utilities;

namespace community.common.Extensions;

/// <summary>
/// Extension methods related to DateOnly properties.
/// </summary>
public static class DateOnlyExtensions
{
    /// <summary>
    /// Converts a nullable DateOnly property to an encrypted string.  
    /// </summary>
    /// <param name="dateOnly">The nullable DateOnly object</param>
    /// <returns>Returns encrypted string if object has value, otherwise null.</returns>
    public static string? ToEncryptedString(this DateOnly? dateOnly) =>
        dateOnly != null ? EncryptionHelper.Encrypt(dateOnly) : null;
    
    /// <summary>
    /// Converts a nullable encrypted string to a DateOnly field
    /// </summary>
    /// <param name="encryptedString"></param>
    /// <returns>Returns a parsed date from encrypted string if the object has value, otherwise null.</returns>
    public static DateOnly? FromEncryptedString(this string? encryptedString) => 
        encryptedString != null ? EncryptionHelper.DecryptAsDateOnly(encryptedString) : null;
}
