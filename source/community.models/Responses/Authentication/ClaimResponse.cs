using community.common.Interfaces;

namespace community.models.Responses.Authentication;

/// <summary>
///     Defines the response to requesting claim data.
/// </summary>
public class ClaimResponse : IResponse
{
    /// <summary>
    ///     Get or sets the name of the claim.
    /// </summary>
    public string Name { get; set; } = "";
}