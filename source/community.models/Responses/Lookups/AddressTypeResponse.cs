using community.data.entities.Lookups;

namespace community.models.Responses.Lookups;

/// <summary>
///     Defines an available address type response.
/// </summary>
public record AddressTypeResponse(
    Guid Id,
    Guid? CommunityId,
    string Name)
{
    /// <summary>
    ///     Maps a database entity to a return record.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static implicit operator AddressTypeResponse(AddressType entity)
    {
        return new AddressTypeResponse(entity.Id, entity.CommunityId, entity.Name);
    }
}