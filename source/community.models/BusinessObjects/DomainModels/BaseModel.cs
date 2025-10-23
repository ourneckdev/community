using System.ComponentModel;
using System.Security.Cryptography;
using System.Text.Json;

namespace community.models.BusinessObjects.DomainModels;

/// <summary>
/// </summary>
public class BaseModel : IChangeTracking
{
    /// <summary>
    ///     Gets or sets the UTC date the record was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the UTC date the record was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    ///     Gets or sets the optional id of the user who created the record.
    /// </summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Gets or sets the optional id of the user who last modified the record.
    /// </summary>
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    ///     Gets or sets the flag indicating if the record has been soft deleted
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void AcceptChanges()
    {
        IsChanged = true;
    }

    /// <summary>
    /// </summary>
    public bool IsChanged { get; private set; }


    /// <summary>
    ///     Computes a hash of the object
    /// </summary>
    /// <returns></returns>
    protected async Task<string> GetHash()
    {
        using var md5 = MD5.Create();
        using var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(JsonSerializer.Serialize(this));
        var hash = await md5.ComputeHashAsync(stream);
        return Convert.ToBase64String(hash);
    }
}