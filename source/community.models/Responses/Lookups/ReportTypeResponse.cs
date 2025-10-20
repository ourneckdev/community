using community.data.entities.Lookups;

namespace community.models.Responses.Lookups;

/// <summary>
/// Defines a report type response object.
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Icon"></param>
/// <param name="SortOrder"></param>
public record ReportTypeResponse(
    Guid Id,
    string Name,
    string Icon,
    short SortOrder)
{
    /// <summary>
    ///     Maps a report type database entity to a response object.
    /// </summary>
    /// <param name="entity">the report type database entity to map</param>
    /// <returns>a hydrated response object.</returns>
    public static implicit operator ReportTypeResponse(ReportType entity)
    {
        return new ReportTypeResponse(entity.Id, entity.Name, entity.Icon, entity.SortOrder);
    }
}