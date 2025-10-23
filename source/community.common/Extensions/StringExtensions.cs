using System.Text.RegularExpressions;
using community.common.Definitions;
using community.common.Exceptions;

namespace community.common.Extensions;

/// <summary>
///     Defines extensions to strings
/// </summary>
public static partial class StringExtensions
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
            return MyRegex().IsMatch(emailAddress);
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
    public static bool IsValidUsPhoneNumber(this string phoneNumber)
    {
        try
        {
            phoneNumber.FormatUsPhoneNumber();
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
    public static string FormatUsPhoneNumber(this string input)
    {
        var phoneNumber = new string(input.Where(char.IsDigit).ToArray());
        return phoneNumber.Length != 10
            ? throw new BusinessRuleException(ValidationMessages.PhoneNumberLength)
            : phoneNumber;
    }

    /// <summary>
    ///     In order to protect the underlying tokens, or any other potentially sensitive data from
    ///     being logged, we're going to mask the values with ellipses to indicate there is additional data.
    /// </summary>
    /// <remarks>
    ///     If an uneven number is provided for the mask length, the first portion of the mask will contain
    ///     the extra character.
    /// </remarks>
    /// <param name="input">the string to be masked</param>
    /// <param name="maxLength">The maximum amount of characters to display.</param>
    /// <returns>the masked string</returns>
    public static string MaskString(this string input, int maxLength = 20)
    {
        if (input.Length <= maxLength)
            return input;

        var frontHalf = (int)decimal.Round((decimal)maxLength / 2);
        var backHalf = maxLength - frontHalf;

        var maskedString = $"{input[..frontHalf]}...{input[^backHalf..]}";
        return maskedString;
    }

    [GeneratedRegex(@"^\w+([-+.']\w+)*@(\[*\w+)([-.]\w+)*\.\w+([-.]\w+\])*$")]
    private static partial Regex MyRegex();
}