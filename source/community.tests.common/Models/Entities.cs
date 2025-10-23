using community.common.Utilities;
using community.data.entities;

namespace community.tests.common.Models;

public static class Entities
{
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly Guid CommunityId = Guid.NewGuid();

    public static Contact ValidHomePhone = new()
    {
        Id = Guid.NewGuid(),
        ContactMethodId = PhoneContactMethods.HomePhone,
        Value = "4052390239",
        UserId = UserId,
        CanContact = true,
        CommunityId = CommunityId,
        IsActive = true,
        CreatedDate = DateTime.UtcNow.AddDays(-new Random().Next(5, 7))
    };

    public static User ValidUser => new()
    {
        Id = UserId,
        Username = "user@example.com",
        Password = EncryptionHelper.Encrypt(nameof(User.Password)),
        UserTypeId = UserTypes.CommunityMember,
        LoginCode = null,
        LoginCodeExpiration = null,
        Addresses = [ValidUserAddress],
        ContactMethods = [ValidHomePhone, ValidPersonalEmail],
        Communities = [ValidCommunity]
    };

    public static Community ValidCommunity => new()
    {
        Id = CommunityId,
        Name = nameof(Community.Name),
        Addresses = [ValidCommunityAddress]
    };

    public static UserAddress ValidUserAddress => new()
    {
        Id = Guid.NewGuid(),
        CommunityId = CommunityId,
        UserId = UserId,
        AddressTypeId = AddressTypes.Home,
        AddressLine1 = nameof(UserAddress.AddressLine1),
        City = nameof(UserAddress.City),
        StateCode = nameof(UserAddress.StateCode),
        CountryCode = nameof(UserAddress.CountryCode),
        PostalCode = nameof(UserAddress.PostalCode),
        CountyCode = nameof(UserAddress.CountyCode),
        Longitude = -104.2302M,
        Latitude = 39.2302M,
        TimeZone = "US/Mountain"
    };

    public static CommunityAddress ValidCommunityAddress => new()
    {
        Id = Guid.NewGuid(),
        CommunityId = CommunityId,
        AddressTypeId = AddressTypes.Primary,
        AddressLine1 = nameof(UserAddress.AddressLine1),
        City = nameof(UserAddress.City),
        StateCode = nameof(UserAddress.StateCode),
        CountryCode = nameof(UserAddress.CountryCode),
        PostalCode = nameof(UserAddress.PostalCode),
        CountyCode = nameof(UserAddress.CountyCode),
        Longitude = -104.2302M,
        Latitude = 39.2302M,
        TimeZone = "US/Mountain"
    };

    public static Contact ValidPersonalEmail => new()
    {
        Id = Guid.NewGuid(),
        ContactMethodId = EmailContactMethods.PersonalEmail,
        Value = "test@example.com",
        UserId = UserId,
        CanContact = true,
        CommunityId = CommunityId,
        IsActive = true,
        CreatedDate = DateTime.UtcNow.AddDays(-new Random().Next(5, 7))
    };
}