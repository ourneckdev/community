using community.common.BaseClasses;

namespace community.models.Responses.Base;

/// <summary>
///     Encapsulates the response of a single value
/// </summary>
/// <param name="Item">the item being returned, typically a guid id</param>
/// <typeparam name="T">The type of the value being returned, typically a guid</typeparam>
public record SingleResponse<T>(T Item) : BaseRecord
{
}