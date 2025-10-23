using community.common.BaseClasses;

namespace community.models.Responses.Base;

/// <summary>
///     Encapsulates the response of a collection of values
/// </summary>
/// <param name="Items">the collection being returned, typically a guid id</param>
/// <typeparam name="T">The type of the value being returned, typically a guid</typeparam>
public record MultiResponse<T>(IEnumerable<T> Items) : BaseRecord
{
    /// <summary>
    /// </summary>
    public long Count => Items.Count();
}