using community.common.BaseClasses;

namespace community.models.Responses.Base;

/// <summary>
///     Wraps an array of values into a response object that includes the correlation id and the execution time
/// </summary>
/// <param name="Values"></param>
/// <typeparam name="T"></typeparam>
public record LookupResponse<T>(IEnumerable<T> Values) : BaseRecord
{
}