using System.ComponentModel;
using community.common.Extensions;

namespace community.common.tests.Extensions;

public class StringExtensionTests
{
    [Theory, Category("Unit")]
    [InlineData("407.662.9827")]
    [InlineData("(407) 662.9827")]
    [InlineData("(407)-662-9827")]
    [InlineData("(407) 662-9827")]
    [InlineData("4076629827")]
    public void ValidateUsPhoneNumber_ShouldSucceed(string phoneNumber)
    {
        Assert.True(phoneNumber.IsValidUsPhoneNumber());
    }
    
    [Theory, Category("Unit")]
    [InlineData("407.662.982")]
    [InlineData("(407) 662.M827")]
    [InlineData("662-9827")]
    [InlineData("+14076629827")]
    public void ValidateUsPhoneNumber_InvalidNumber_ShouldSucceed(string phoneNumber)
    {
        Assert.False(phoneNumber.IsValidUsPhoneNumber());
    }

    [Theory, Category("Unit")]
    [InlineData("407.662.9827", "4076629827")]
    [InlineData("(407) 662.9827", "4076629827")]
    [InlineData("(407)-662-9827", "4076629827")]
    [InlineData("(407) 662-9827", "4076629827")]
    [InlineData("4076629827", "4076629827")]
    public void FormatUsPhoneNumber_ShouldSucceed(string phoneNumber, string expectedResult)
    {
        var result = phoneNumber.FormatUsPhoneNumber();
        Assert.Equal(expectedResult, result);
    }

    [Theory, Category("Unit")]
    [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", "eyJhbGciOi...I6IkpXVCJ9")]
    [InlineData("35YZxwK6E9zsKjNIM5UIjQE2O3G2TqQW9LE60=", "35YZxwK6E9...TqQW9LE60=")]
    public void MaskString_ReturnsMaskedString_ShouldSucceed(string maskedString, string expectedResult)
    {
        var result = maskedString.MaskString(); 
        Assert.Equal(expectedResult, result);
    }
    
    [Theory, Category("Unit")]
    [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", 20, "eyJhbGciOi...I6IkpXVCJ9")]
    [InlineData("35YZxwK6E9zsKjNIM5UIjQE2O3G2TqQW9LE60=", 20, "35YZxwK6E9...TqQW9LE60=")]
    [InlineData("35YZxwK6E9zsKjNIM5UIjQE2O3G2TqQW9LE60=", 10, "35YZx...LE60=")]
    [InlineData("35YZxwK6E9zsKjNIM5UIjQE2O3G2TqQW9LE60=", 15, "35YZxwK6...W9LE60=")]
    public void MaskString_VaryingMaskLength_ReturnsMaskedString_ShouldSucceed(string maskedString, int maskLength, string expectedResult)
    {
        var result = maskedString.MaskString(maskLength); 
        Assert.Equal(expectedResult, result);
    }

    [Theory, Category("Unit")]
    [InlineData("sample@example.com")]
    [InlineData("sample@example.co")]
    [InlineData("sample@example.co.uk")]
    [InlineData("sample.lastname@example.com")]
    [InlineData("sample.lastname+1@example.com")]
    [InlineData("sample.lastname+1@example.plus1.com")]
    public void ValidateEmail_ShouldSucceed(string email)
    {
        Assert.True(email.IsValidEmailAddress());
    }

    [Theory, Category("Unit")]
    [InlineData("-sample-@example.com")]
    [InlineData(".sample@example.co")]
    [InlineData("sample..lastname@example.com")]
    [InlineData("sample#lastname+1@example.com")]
    [InlineData("sample.lastname+@example.plus1.com")]
    public void ValidateEmail_InvalidFormats_ReturnsFalse_ShouldSucceed(string email)
    {
        Assert.False(email.IsValidEmailAddress());
    }
}