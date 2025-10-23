using System.Diagnostics.CodeAnalysis;

namespace community.common.BaseClasses;

/// <summary>
///     Defines default properties on the immutable return record.
/// </summary>
[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public abstract record BaseRecord
{
    /// <summary>
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// </summary>
    public long ExecutionMilliseconds { get; set; }
}