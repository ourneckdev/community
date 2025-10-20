using community.common.BaseClasses;

namespace community.models.Responses.Base;

/// <summary>
///     Represents response items for primary tables with an ID.
/// </summary>
public abstract class BasePrimaryResponse : BaseResponse
{
    /// <summary>
    ///     Gets or sets the database identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Maps the base values for primary entities (entities that are defined by  single id)
    /// </summary>
    /// <param name="source">The entity being mapped.</param>
    /// <typeparam name="T">The implementing type.</typeparam>
    /// <returns>A hydrated base level data.</returns>
    protected static T MapValues<T>(BasePrimaryEntity source) where T : BasePrimaryResponse, new()
    {
        var baseResponse = BaseResponse.MapValues<T>(source);
        baseResponse.Id = source.Id;
        return baseResponse;
    }
}