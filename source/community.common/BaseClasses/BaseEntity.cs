using community.common.Interfaces;

namespace community.common.BaseClasses;

/// <summary>
///     Abstract abstract class implementing the Audit columns for all entities
/// </summary>
public abstract class BaseEntity : IDatabaseEntity
{
    /// <inheritdoc />
    public DateTime CreatedDate { get; set; }

    /// <inheritdoc />
    public DateTime ModifiedDate { get; set; }

    /// <inheritdoc />
    public Guid? CreatedBy { get; set; }

    /// <inheritdoc />
    public Guid? ModifiedBy { get; set; }

    /// <inheritdoc />
    public bool IsActive { get; set; }
}