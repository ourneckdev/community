using System.ComponentModel.DataAnnotations;
using community.common.BaseClasses;
using community.data.entities.Lookups;

namespace community.data.entities;

/// <summary>
///     Represents a user of the system
/// </summary>
public sealed class User : BasePrimaryEntity
{
    /// <summary>
    ///     Gets or sets the type of the user
    /// </summary>
    /// <remarks>
    ///     SiteAdmin
    ///     SupportAdmin
    ///     CommunityAdmin
    ///     CommunityMember
    /// </remarks>
    
    public Guid UserTypeId { get; set; }

    /// <summary>
    ///     Gets or sets the username, either email or mobile phone, of the user.
    /// </summary>
    
    [MaxLength(75)]
    public string Username { get; set; } = "";

    /// <summary>
    ///     Gets or sets an optional password that can be used to login, if preferred.
    /// </summary>
    
    [MaxLength(150)]
    public string? Password { get; set; } = "";
    
    /// <summary>
    ///     Gets or sets an indicator as to whether the user is a verified member of the community.
    /// </summary>
    
    public bool UsernameVerified { get; set; }

    /// <summary>
    ///     Gets or sets the date the user was verified, if verified.
    /// </summary>
    
    public DateTime? UsernameVerifiedDate { get; set; }

    /// <summary>
    ///     Gets or sets the temporary password code used for logging in.
    /// </summary>
    /// <remarks>
    ///     While this column exists it will never be selected or set by object reference.  It will be used in query
    ///     predicates only during login.
    /// </remarks>
    
    [Length(6, 6)]
    public string? LoginCode { get; set; }

    /// <summary>
    ///     Get or sets the expiration time for the generated password code.
    /// </summary>
    /// <remarks>
    ///     While this column exists it will never be selected or set by object reference.  It will be used in query
    ///     predicates only during login.
    /// </remarks>
    
    public DateTime? LoginCodeExpiration { get; set; }

    /// <summary>
    ///     Gets or sets the user's name prefix.
    /// </summary>
    
    [MaxLength(10)]
    public string? Prefix { get; set; }

    /// <summary>
    ///     Gets or sets the user's first name
    /// </summary>
    
    [MaxLength(100)]
    public string FirstName { get; set; } = "";

    /// <summary>
    ///     Gets or sets the user's lastname.
    /// </summary>
    
    [MaxLength(100)]
    public string LastName { get; set; } = "";

    /// <summary>
    ///     Gets or sets the user's name prefix.
    /// </summary>
    
    [MaxLength(10)]
    public string? Suffix { get; set; }

    /// <summary>
    ///     Gets or sets the user's date of birth as an encrypted string.
    /// </summary>
    
    public string? DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets a status indicating whether the user's account is locked out.
    /// </summary>
    /// <remarks>Locked accounts are a result of too many failed login attempts.</remarks>
    
    public bool Locked { get; set; }

    /// <summary>
    ///     Gets or sets the object name in S3
    /// </summary>
    
    [MaxLength(255)]
    public string? ProfilePic { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp the last time the user logged in
    /// </summary>
    
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    ///     Gets or sets the last community the user logged into
    /// </summary>
    
    public Guid? LastCommunityId { get; set; }


    /// <summary>
    ///     Gets the email addresses and phone numbers for the user.
    /// </summary>
    public IEnumerable<Contact>? ContactMethods { get; set; } = new List<Contact>();

    /// <summary>
    ///     Gets the addresses for the user.
    /// </summary>
    public IEnumerable<UserAddress>? Addresses { get; set; } = new List<UserAddress>();

    /// <summary>
    ///     Gets the communities the user is assigned to.
    /// </summary>
    public IEnumerable<Community>? Communities { get; set; } = new List<Community>();


    #region Navigation Properties

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public UserType UserType { get; set; } = null!;

    /// <summary>
    ///     Navigation property.
    /// </summary>
    public Community LastCommunity { get; set; } = null!;

    #endregion
}