// ReSharper disable InconsistentNaming

namespace community.common.Definitions;

/// <summary>
///     Encapsulates the error codes thrown by the application.
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    ///     Login Code Expired
    /// </summary>
    public const string Login_LoginCodeExpired = "Login code expired or invalid.";

    /// <summary>
    ///     Incorrect password.
    /// </summary>
    public const string Login_IncorrectPassword = "Username or password is incorrect.";


    /// <summary>
    ///     User not found
    /// </summary>
    public const string User_UserNotFound = "User does not exist.";

    /// <summary>
    ///     Failed to return GeoCode Data from GoogleSettings API.
    /// </summary>
    public const string Google_FailedToReturnGeoCodeData = "Failed to return GeoCode Data from GoogleSettings API.";

    /// <summary>
    ///     Address not found for community id.
    /// </summary>
    public const string Address_NotFound_Community = "Address not found for community {0}.";

    /// <summary>
    ///     Address not found for community and user id.
    /// </summary>
    public const string Address_NotFound_User = "Address not found for community {0} and user {1}.";

    /// <summary>
    ///     Contact not found
    /// </summary>
    public const string Contact_NotFound = "Contact not found.";

    /// <summary>
    ///     Failed to register community
    /// </summary>
    public const string RegistrationError_Community = "Failed to register commmunity.";

    
    /// <summary>
    ///     Failed to add contact.
    /// </summary>
    public const string DatabaseError_AddContact = "An unexpected error occurred adding a contact.";

    /// <summary>
    ///     An error occurred adding a community.
    /// </summary>
    public const string DatabaseError_AddCommunity = "An unexpected error occurred adding a community.";

    /// <summary>
    ///     An unexpected error occurred adding a user to a commmunity.
    /// </summary>
    public const string DatabaseError_AddUserToCommunity =
        "An unexpected error occurred adding a user to a commmunity.";

    /// <summary>
    ///     An unexpected error occurred adding a contact_consent_log record.
    /// </summary>
    public const string DatabaseError_AddContactConsentLog =
        "An unexpected error occurred adding a contact_consent_log record.";

    /// <summary>
    ///     An unexpected error occurred listing community contacts.
    /// </summary>
    public const string DatabaseError_ListCommunityContacts =
        "An unexpected error occurred listing community contacts.";

    /// <summary>
    ///     An unexpected error occurred listing user contacts.
    /// </summary>
    public const string DatabaseError_ListUserContacts = "An unexpected error occurred listing user contacts.";

    /// <summary>
    ///     An unexpected error occurred getting contact record.
    /// </summary>
    public const string DatabaseError_GetContact = "An unexpected error occurred getting contact record.";

    /// <summary>
    ///     An unexpected error occurred updationg a contact record.
    /// </summary>
    public const string DatabaseError_UpdateContact = "An unexpected error occurred updating a contact record.";

    /// <summary>
    ///     An unexpected error occurred adding a community address.
    /// </summary>
    public const string DatabaseError_AddCommunityAddress = "An unexpected error occurred adding a community address.";

    /// <summary>
    ///     "An unexpected error occurred adding a user address.";
    /// </summary>
    public const string DatabaseError_AddUserAddress = "An unexpected error occurred adding a user address.";

    /// <summary>
    ///     An unexpected error occurred updating a user address.
    /// </summary>
    public const string DatabaseError_UpdateUserAddress = "An unexpected error occurred updating a user address.";

    /// <summary>
    ///     An unexpected error occurred updating a community address.
    /// </summary>
    public const string DatabaseError_UpdateCommunityAddress =
        "An unexpected error occurred updating a community address.";

    /// <summary>
    /// An unexpected error occurred retrieving a user address.
    /// </summary>
    public const string DatabaseError_GetUserAddress = "An unexpected error occurred retrieving a user address.";
    
    /// <summary>
    /// An unexpected error occurred retrieving a user address.
    /// </summary>
    public const string DatabaseError_GetCommunityAddress = "An unexpected error occurred retrieving a community address.";

    /// <summary>
    /// An unexpected error occurred listing user addresses.
    /// </summary>
    public const string DatabaseError_ListUserAddresses = "An unexpected error occurred listing user addresses.";
    
    /// <summary>
    /// An unexpected error occurred listing user addresses.
    /// </summary>
    public const string DatabaseError_ListCommunityAddresses = "An unexpected error occurred listing community addresses.";

    /// <summary>
    /// Unexpected error executing search.
    /// </summary>
    public const string DatabaseError_SearchFailed = "Unexpected error executing search.";
}