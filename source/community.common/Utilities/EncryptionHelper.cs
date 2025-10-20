using System.Security.Cryptography;
using System.Text;

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
    public static string Generate(int byteSize = ByteLength)
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(byteSize));
    }

    /// <summary>
    ///     Encrypts plain text into a base-64 representation of resulting byte array.
    /// </summary>
    /// <param name="valueToEncrypt">The plain text to encrypt.</param>
    /// <returns>An encrypted byte array in base 64.</returns>
    public static string Encrypt(object valueToEncrypt)
    {
        var salt = Generate(16);

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
        return $"{salt.Substring(0, Base64StringLength - 2)}{ivString}{encryptedData}";
    }

    /// <summary>
    ///     Decrypts a cipher envelope.
    /// </summary>
    /// <param name="cipherText">The base-64 string containing the encrypted key, encrypted IV and encrypted text.</param>
    /// <returns>The decrypted plain text data.</returns>
    public static string Decrypt(string cipherText)
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
    public static DateOnly DecryptAsDateOnly(string cipherText)
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

        return DateOnly.Parse(data);
    }
}