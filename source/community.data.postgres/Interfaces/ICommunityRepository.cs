using community.common.Interfaces;
using community.data.entities;
using community.data.entities.Search;

namespace community.data.postgres.Interfaces;

/// <summary>
/// Exposes methods for manipulating data related to communities
/// </summary>
public interface ICommunityRepository : IRepository
{
    /// <summary>
    /// First step in initializing a new community
    /// </summary>
    /// <param name="community">The necessary data to create the community.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The id of the newly created community.</returns>
    Task<Guid> AddAsync(Community community, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a relationship between a user and a community
    /// </summary>
    /// <param name="userId">The id of the user wishing to join the community.</param>
    /// <param name="communityId">The id of the community they wish to join</param>
    /// <param name="cancellationToken"></param>
    /// <returns>true/false indicating success.</returns>
    Task<bool> AddUserToCommunityAsync(Guid userId, Guid communityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for an existing community by name, address and phone
    /// </summary>
    /// <param name="search"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IList<CommunitySearchResults>> FindCommunityAsync(FindCommunityRecord search,
        CancellationToken cancellationToken = default);
}