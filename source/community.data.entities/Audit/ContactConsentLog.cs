using community.common.BaseClasses;

namespace community.data.entities.audit;

/// <summary>
/// represents a row of granting or revoking consent to contact for a user or community contact record. 
/// </summary>
public class ContactConsentLog : BaseCommunityEntity
{   
    /// <summary>
    /// Represents whether consent was granted or revoked 
    /// </summary>
    public bool HasConsent { get; set; }
    
    /// <summary>
    /// The timestamp the consent was granted or revoked.
    /// </summary>
    public DateTime ConsentDate { get; set; }
}