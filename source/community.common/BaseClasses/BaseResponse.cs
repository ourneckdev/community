using System.Text.Json.Serialization;
using community.common.Interfaces;

namespace community.common.BaseClasses;

/// <summary>
///     Abstract response class implementing the default properties all objects will encompass.
/// </summary>
public abstract class BaseResponse : IResponse
{
    /// <summary>
    ///     Gets or sets the timestamp the record was initially created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp the record was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    ///     Optionally, gets or sets the id of the user who created the record.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Optionally, gets or sets the id of the user who last modified the record.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    ///     Gets or sets a flag indicating if the record has been soft deleted.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    ///     Mapss the base level data for all entities to a response object.
    /// </summary>
    /// <param name="entity">The base entity being mapped.</param>
    /// <typeparam name="T">The implementing type.</typeparam>
    /// <returns>A base object with the mapped values.</returns>
    protected static T MapValues<T>(BaseEntity entity) where T : BaseResponse, new()
    {
        return new T
        {
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate,
            IsActive = entity.IsActive,
            CreatedBy = entity.CreatedBy,
            ModifiedBy = entity.ModifiedBy
        };
    }
}