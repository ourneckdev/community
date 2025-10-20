using community.data.entities;

namespace community.models.Responses;

/// <summary>
/// 
/// </summary>
public class UserAddressResponse : AddressResponse
{
    /// <summary>
    /// Gets or sets the user id the address is related to.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userAddress"></param>
    /// <returns></returns>
    public static implicit operator UserAddressResponse(UserAddress userAddress)
    {
        var response = MapValues<UserAddressResponse>(userAddress);
        response.UserId = userAddress.UserId;
        return response;
    }
};