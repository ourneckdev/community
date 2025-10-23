using System.ComponentModel;
using community.common.Utilities;

namespace community.common.tests.Extensions;

public class EncryptionHelperTests
{
    [Fact]
    [Category("Unit")]
    public void EncryptAndDecryptTest_ShouldSucceed()
    {
        // using the maximum password length of 32 characters here
        // this helped me determine the length of the string generated from the encryption routine
        // which is 110 characters.
        const string plainText = "BlahBlahBlah98!";
        var encryptedString = EncryptionHelper.Encrypt(plainText);
        var decryptedString = EncryptionHelper.Decrypt(encryptedString);
        Assert.Equal(plainText, decryptedString);
    }

    [Fact]
    [Category("Unit")]
    public void EncryptAndDecryptDates_ShouldSucceed()
    {
        var dateOnly = new DateOnly(2001, 9, 11);
        var encryptedDate = EncryptionHelper.Encrypt(dateOnly);
        var decryptedDate = DateOnly.Parse(EncryptionHelper.Decrypt(encryptedDate));
        Assert.Equal(dateOnly, decryptedDate);
    }

    [Theory]
    [Category("Unit")]
    [InlineData(24)]
    [InlineData(36)]
    [InlineData(48)]
    [InlineData(96)]
    [InlineData(128)]
    public void Generate_256BitString_ShouldSucceed(int bytes)
    {
        var key = EncryptionHelper.Generate(bytes);
        Assert.NotNull(key);
        Assert.Equal(bytes.CalculateBase64Bytes(), key.Length);
    }
}