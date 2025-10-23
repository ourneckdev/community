using community.common.Interfaces;
using community.data.entities;

namespace community.data.postgres.Interfaces;

/// <summary>
///     defines methods required during the registration process.
/// </summary>
public interface IRegistrationRepository : IRepository
{
    /// <summary>
    ///     Responsible for registering a community.
    ///     Create a new community record, a new community admin and any peripheral data supplied.
    /// </summary>
    /// <param name="community"></param>
    /// <returns></returns>
    Task RegisterCommunityAsync(Community community);

    /// <summary>
    ///     Registers a new user against a community.
    /// </summary>
    /// <returns></returns>
    Task RegisterUserAsync(User user);
}