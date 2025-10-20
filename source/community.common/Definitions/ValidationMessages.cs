using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("community.providers.community")]
[assembly:InternalsVisibleTo("community.providers.auth")]
[assembly:InternalsVisibleTo("community.models")]
[assembly:InternalsVisibleTo("community.middleware")]
namespace community.common.Definitions;

internal static class ValidationMessages
{
    internal const string ValidationErrors = "One or more validation errors occurred.";
    internal const string UsernameRequired = "Username is required.";
    internal const string UserNameInvalid = "Username should be either a valid email address or mobile phone number.";
    internal const string LoginCodeRequired = "Login code is required.";
    internal const string PasswordRequired = "Password is required.";
    internal const string FirstNameRequired = "First name is required.";
    internal const string LastNameRequired = "Last name is required.";
    internal const string NameLength = "Name must be more than {0} characters";
    internal const string PhoneNumberLength = "Phone numbers must be 10 digits.";
    internal const string VerificationCodeNull = "Verification code must not be null.";
    internal const string AddressValidationErrors = "One or more Address validation errors occurred.";
    internal const string StreetAddressRequired = "Street address is required.";
    internal const string CityRequired = "City is required";
    internal const string StateRequired = "State/Province is required.";
    internal const string PostalCodeRequired = "Postal code is required.";
    internal const string CountryRequired = "Country is required";
    internal const string CommunityNameIsRequired = "Community name is required.";
    internal const string PrimaryAddressRequired = "Primary address is required.";
    internal const string InvalidWebAddress = "Invalid web address.";
    internal const string InvalidWebAddressScheme = "Websites must use HTTPS";
    internal const string ContactMethodRequired = "Primary contact method is required.";
    internal const string UserExists = "Username is already registered.";
    internal const string ContactMethodInvalid = "Must be valid phone or email address.";
    internal const string TimeZoneRequired = "Time zone is required.";
    internal const string PotentialDuplicateCommunity = "One or more existing communities found.";
}