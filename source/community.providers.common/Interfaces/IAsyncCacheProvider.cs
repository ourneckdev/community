namespace community.providers.common.Interfaces;

/// <summary>
/// </summary>
public interface IAsyncCacheProvider<T> where T : class
{
    /// <summary>
    ///     Retrieves a value from cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetAsync(string key);

    /// <summary>
    ///     Sets an item into memory cache.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    Task SetAsync(string key, T value);
}