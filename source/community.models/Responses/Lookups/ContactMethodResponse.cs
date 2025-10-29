using community.common.Enumerations;
using community.data.entities.Lookups;

namespace community.models.Responses.Lookups;

/// <summary>
///     Defines an available address type response.
/// </summary>
public record ContactMethodResponse(
    Guid Id,
    Guid? CommunityId,
    ContactType ContactType,
    string Name)
{
    /// <summary>
    ///     Maps a database entity to a return record.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static implicit operator ContactMethodResponse(ContactMethod entity)
    {
        return new ContactMethodResponse(entity.Id, 
            entity.CommunityId,
            Enum.Parse<ContactType>(entity.ContactType), 
            entity.Name);
    }
}