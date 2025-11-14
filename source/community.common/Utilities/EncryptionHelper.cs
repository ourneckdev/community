using System.Security.Cryptography;
using System.Text;
using community.common.Extensions;
using Microsoft.Extensions.Configuration;

namespace community.common.Utilities;

/// <summary>
///     Static implementation for working with securing string data.
/// </summary>
public static class EncryptionHelper
{
    private const int ByteLength = 32;
    private const int Base64StringLength = 24;
    private const string SaltPadding = "==";

    /// <summary>
    ///     Generates a cryptographically secure string.
    /// </summary>
    /// <returns>The generated string used as a refresh token.</returns>
    public static string GenerateSalt(int byteSize = ByteLength)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(byteSize));
    }    
    
    /// <summary>
    /// Generates a salt hash from a supplied key
    /// </summary>
    /// <param name="salt"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Generate(string salt)
    {
        if(string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));
        var bytes = Encoding.UTF8.GetBytes(salt);
        using var sha = SHA512.Create();
        var hash = Convert.ToBase64String(sha.ComputeHash(bytes))[..16];
        return hash;
    }

    /// <summary>
    ///     Calculates the storage requirements for a base65 encoded cryptographic strong random numbers based on length of
    ///     bytes required.
    /// </summary>
    /// <remarks>
    ///     Base64 encoding causes an overhead of 33–37% relative to the size of the original binary data
    /// </remarks>
    /// <param name="byteSize">The length of the bytes required to generate.</param>
    /// <returns>The calculated byte size of the encoded cryptographically strong random number.</returns>
    public static int CalculateBase64Bytes(this int byteSize)
    {
        return Convert.ToInt32(4 * Math.Ceiling(Convert.ToDecimal(byteSize) / 3));
    }

    /// <summary>
    ///     Encrypts plain text into a base-64 representation of resulting byte array.
    /// </summary>
    /// <param name="valueToEncrypt">The plain text to encrypt.</param>
    /// <returns>An encrypted byte array in base 64.</returns>
    public static string Encrypt(object valueToEncrypt)
    {
        var salt = ConfigurationHelper.Configuration.GetValue<string>("Encryption:Salt") 
                   ?? throw new ArgumentNullException(nameof(valueToEncrypt));
        
        using var aes = Aes.Create();
        aes.GenerateIV();
        aes.Key = Convert.FromBase64String(salt);
        var iv = aes.IV;
        var ivString = Convert.ToBase64String(iv);

        using var ms = new MemoryStream();
        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream, Encoding.UTF8))
        {
            writer.Write(valueToEncrypt);
        }

        var encryptedData = Convert.ToBase64String(ms.ToArray());
        return $"{ivString}{encryptedData}";
    }

    /// <summary>
    ///     Decrypts a cipher envelope.
    /// </summary>
    /// <param name="cipherText">The base-64 string containing the encrypted key, encrypted IV and encrypted text.</param>
    /// <exception cref="ArgumentNullException">Throws an argument null exception if the salt is missing from secrets.</exception>
    /// <returns>The decrypted plain text data.</returns>
    public static string Decrypt(string cipherText)
    {
        if(string.IsNullOrEmpty(cipherText)) return string.Empty;
        
        var (salt, iv, encryptedData) = GetParts(cipherText);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(salt);
        aes.IV = Convert.FromBase64String(iv);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(encryptedData));
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);
        return reader.ReadToEnd();
    }
    
    /// <summary>
    ///     Decrypts a cipher envelope.
    /// </summary>
    /// <param name="cipherText">The base-64 string containing the encrypted key, encrypted IV and encrypted text.</param>
    /// <returns>The decrypted plain text data.</returns>
    [Obsolete("Moved to a protected salt.")]
    public static string DecryptWithStoredSalt(string cipherText)
    {
        var salt = $"{cipherText.Substring(0, Base64StringLength - 2)}{SaltPadding}";
        var iv = cipherText.Substring(Base64StringLength - 2, Base64StringLength);
        var encryptedData = cipherText.Substring(Base64StringLength * 2 - 2);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(salt);
        aes.IV = Convert.FromBase64String(iv);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(encryptedData));
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);
        var data = reader.ReadToEnd();

        return data;
    }

    /// <summary>
    ///     Decrypts a cipher envelope and parses the result to a DateOnly struct.
    /// </summary>
    /// <param name="cipherText">The base-64 string containing the encrypted key, encrypted IV and encrypted text.</param>
    /// <returns>The decrypted plain text data.</returns>
    public static DateOnly DecryptAsDateOnly(string cipherText) => DateOnly.Parse(Decrypt(cipherText));
    
    private static (string salt, string iv, string encryptedData) GetParts(this string cipherText)
    {
        var salt = ConfigurationHelper.Configuration.GetValue<string>("Encryption:Salt") ?? throw new ArgumentNullException("Encryption:Salt");
        var iv = cipherText[..Base64StringLength];
        var encryptedData = cipherText[Base64StringLength..];

        return (salt, iv, encryptedData);
    }
}