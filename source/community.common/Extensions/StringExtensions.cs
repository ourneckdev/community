using System.Net.Mail;
using community.common.Definitions;
using community.common.Exceptions;

namespace community.common.Extensions;

/// <summary>
///     Defines extensions to strings
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Reads a PEM file and retrieves the byetes of the key.
    /// </summary>
    /// <param name="pemString"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    public static byte[]? GetBytesFromPem(this string pemString, string section)
    {
        var header = $"-----BEGIN {section}-----";
        var footer = $"-----END {section}-----";

        var start = pemString.IndexOf(header, StringComparison.Ordinal);
        if (start < 0)
            return null;

        start += header.Length;
        var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

        return end < 0 ? null : Convert.FromBase64String(pemString.Substring(start, end));
    }

    /// <summary>
    ///     Accepts a string and attempt to validate if it's an actual email address
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public static bool IsValidEmailAddress(this string emailAddress)
    {
        try
        {
            // ReSharper disable once ObjectCreationAsStatement
            new MailAddress(emailAddress);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Validates a phone number length.
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        try
        {
            phoneNumber.FormatPhoneNumber();
            return true;
        }
        catch (BusinessRuleException)
        {
            return false;
        }
    }

    /// <summary>
    ///     Strips all non-numeric values from a supplied string and validates the string length.
    /// </summary>
    /// <param name="input">The input to be converted to numeric characters and validated for length.</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="BusinessRuleException">Throws when the phone number is incorrect length.</exception>
    public static string FormatPhoneNumber(this string input)
    {
        var phoneNumber = new string(input.Where(char.IsDigit).ToArray());
        if (phoneNumber.Length != 10)
            throw new BusinessRuleException(ValidationMessages.PhoneNumberLength);
        return phoneNumber;
    }
}