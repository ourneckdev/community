using community.common.Utilities;

namespace community.common.tests;

public class EncryptionHelperTests
{
    [Fact]
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
    public void EncryptAndDecryptDates_ShouldSucceed()
    {
        var dateOnly = new DateOnly(2001, 9, 11);
        var encrytpedDate = EncryptionHelper.Encrypt(dateOnly);
        var decryptedDate = DateOnly.Parse(EncryptionHelper.Decrypt(encrytpedDate));
        Assert.Equal(dateOnly, decryptedDate);
    }

    [Fact]
    public void Generate_256BitString_ShouldSucceed()
    {
        var key = EncryptionHelper.Generate(24);
        Assert.NotNull(key);
    }
}