using community.data.entities.Lookups;

namespace community.models.Responses.Lookups;

/// <summary>
///     Defines an available address type response.
/// </summary>
public record ParcelSizeUnitResponse(
    Guid Id,
    string Name,
    string? Description)
{
    /// <summary>
    /// Maps a database entity to a return record.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static implicit operator ParcelSizeUnitResponse(ParcelSizeUnit entity)
    {
        return new ParcelSizeUnitResponse(entity.Id, entity.Name, entity.Description);
    }
}