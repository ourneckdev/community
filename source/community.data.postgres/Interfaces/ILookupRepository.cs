using community.common.BaseClasses;
using community.common.Interfaces;

namespace community.data.postgres.Interfaces;

/// <summary>
///     Defines methods available to the lookup repository, for retrieving values meant to display in selection lists
/// </summary>
public interface ILookupRepository : IRepository
{
    /// <summary>
    ///     Lists types that implememnt a commmunity id.
    /// </summary>
    /// <param name="communityId">Optional community id</param>
    /// <returns></returns>
    Task<IEnumerable<T>> ListAsync<T>(Guid? communityId = null) where T : BaseLookupEntity;
}