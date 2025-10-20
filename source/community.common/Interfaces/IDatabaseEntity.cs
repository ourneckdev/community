namespace community.common.Interfaces;

/// <summary>
///     Defines all entities used throughout the application.
/// </summary>
public interface IDatabaseEntity
{
    /// <summary>
    ///     Defines when the record was originally created
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Defines when the record was last modified
    /// </summary>
    DateTime ModifiedDate { get; set; }

    /// <summary>
    ///     Defines the user who originally created the record.
    /// </summary>
    Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Defines the last user that modified the record.
    /// </summary>
    Guid? ModifiedBy { get; set; }

    /// <summary>
    ///     A flag indicating if the record has been soft-deleted.
    /// </summary>
    bool IsActive { get; set; }
}