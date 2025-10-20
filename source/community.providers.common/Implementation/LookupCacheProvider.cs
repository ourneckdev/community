using community.common.BaseClasses;
using community.providers.common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace community.providers.common.Implementation;


//, ILookupRepository lookupRepository/// <inheritdoc />
/// <summary>
/// 
/// </summary>
/// <param name="cache"></param>
/// <typeparam name="T"></typeparam>
public class LookupCacheProvider<T>(IMemoryCache cache) : IAsyncCacheProvider<T> where T : BaseEntity
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    
    /// <inheritdoc />
    public async Task<T?> GetAsync(string key)
    {
        await Semaphore.WaitAsync();
        try
        {
            cache.TryGetValue(key, out T? value);
            return value;
        }
        finally
        {
            Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public async Task SetAsync(string key, T value)
    {
        await Semaphore.WaitAsync();
        try
        {
            cache.Set(key, value);
        }
        finally
        {
            Semaphore.Release();
        }
    }
} 