using community.data.entities.Lookups;

namespace community.models.Responses.Lookups;

/// <summary>
///     An immutable UserType response
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
public record UserTypeResponse(
    Guid Id,
    string Name,
    string Description)
{
    /// <summary>
    ///     Maps a user type database entity to a response object.
    /// </summary>
    /// <param name="entity">the database report type entity.</param>
    /// <returns>a hydrated response object.</returns>
    public static implicit operator UserTypeResponse(UserType entity)
    {
        return new UserTypeResponse(entity.Id, entity.Name, entity.Description);
    }
}