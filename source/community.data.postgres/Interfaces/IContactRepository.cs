using community.common.Interfaces;
using community.data.entities;

namespace community.data.postgres.Interfaces;

/// <summary>
/// Defines the publicly available methods for manipulating contact method data.
/// </summary>
public interface IContactRepository : IRepository
{
    /// <summary>
    /// Adds a user contact method record
    /// </summary>
    /// <param name="contact">The entity to save</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The ID of the newly created record.</returns>
    Task<Guid> AddAsync(Contact contact, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a contact method and saves a log of their consent.
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single contact record
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Contact> GetAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// List contacts for a community
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Contact>> ListAsync(Guid communityId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// List contacts for a user.
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Contact>> ListAsync(Guid communityId, Guid userId, CancellationToken cancellationToken = default);
}