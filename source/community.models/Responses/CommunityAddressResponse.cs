using community.data.entities;

namespace community.models.Responses;

/// <summary>
///     Community address concrete implementation
/// </summary>
public class CommunityAddressResponse : AddressResponse
{
    /// <summary>
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static implicit operator CommunityAddressResponse(CommunityAddress address)
    {
        var response = MapValues<CommunityAddressResponse>(address);
        return response;
    }
}