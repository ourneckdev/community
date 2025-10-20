namespace community.models.BusinessObjects.DomainModels;

/// <summary>
/// Defines a priomary domain object
/// </summary>
public class BasePrimaryModel : BaseModel
{
    /// <summary>
    /// Gets or sets the ID of the object.
    /// </summary>
    protected Guid Id { get; set; }
}