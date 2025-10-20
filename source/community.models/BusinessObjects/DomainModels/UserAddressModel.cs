using community.data.entities;
using community.models.Requests.Registration;

namespace community.models.BusinessObjects.DomainModels;

/// <summary>
/// 
/// </summary>
public class UserAddressModel : AddressModel
{
    /// <summary>
    /// Gets ir sets the user id                                                                                                
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public Guid AddressTypeId { get; set; }

    /// <summary>
    /// Maps a user address to a domain entity
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static implicit operator UserAddressModel(UserAddress address) =>
        new()
        {
            Id = address.Id,
            CommunityId = address.CommunityId,
            UserId = address.UserId,
            AddressTypeId = address.AddressTypeId,
            LotNumber = address.LotNumber,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            AddressLine3 = address.AddressLine3,
            City = address.City,
            StateCode = address.StateCode,
            PostalCode = address.PostalCode,
            CountyCode = address.CountyCode,
            CountryCode = address.CountryCode,
            Longitude = address.Longitude,
            Latitude = address.Latitude,
            TimeZone = address.TimeZone,
            TimeZoneOffset = address.TimeZoneOffset,
            CreatedDate = address.CreatedDate,
            ModifiedDate = address.ModifiedDate,
            CreatedBy = address.CreatedBy,
            ModifiedBy = address.ModifiedBy,
            IsActive = address.IsActive
        };   
    
    /// <summary>
    /// Maps a user address to a domain entity
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public static implicit operator UserAddress(UserAddressModel address) =>
        new()
        {
            Id = address.Id,
            CommunityId = address.CommunityId,
            UserId = address.UserId,
            AddressTypeId = address.AddressTypeId,
            LotNumber = address.LotNumber,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            AddressLine3 = address.AddressLine3,
            City = address.City,
            StateCode = address.StateCode,
            PostalCode = address.PostalCode,
            CountyCode = address.CountyCode,
            CountryCode = address.CountryCode,
            Longitude = address.Longitude,
            Latitude = address.Latitude,
            TimeZone = address.TimeZone,
            TimeZoneOffset = address.TimeZoneOffset,
            CreatedDate = address.CreatedDate,
            ModifiedDate = address.ModifiedDate,
            CreatedBy = address.CreatedBy,
            ModifiedBy = address.ModifiedBy,
            IsActive = address.IsActive
        };

    /// <summary>
    /// Applies changes from a registration address request over the top of a database enitty.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    public async Task ApplyChanges(RegisterAddressRequest request, Guid? userId)
    {
        var hash = await GetHash();
        
        if(!LotNumber?.Equals(request.LotNumber) ?? false)
            LotNumber = request.LotNumber;
        
        if(!AddressLine1.Equals(request.AddressLine1))
            AddressLine1 = request.AddressLine1;
        
        if(!AddressLine2?.Equals(request.AddressLine2) ?? false)
            AddressLine2 = request.AddressLine2;
        
        if(!AddressLine3?.Equals(request.AddressLine3) ?? false)
            AddressLine3 = request.AddressLine3;
        
        if(!City.Equals(request.City))
            City = request.City;
        
        if(!StateCode.Equals(request.StateCode))
            StateCode = request.StateCode;
        
        if(!PostalCode.Equals(request.PostalCode))
            PostalCode = request.PostalCode;
        
        if(!CountyCode?.Equals(request.CountyCode) ?? false)
            CountyCode = request.CountyCode;
        
        if(!CountryCode.Equals(request.CountryCode))
            CountryCode = request.CountryCode;
        
        if(!Longitude.Equals(request.Longitude))
            Longitude = request.Longitude;
        
        if(!Latitude.Equals(request.Latitude))
        
            Latitude = request.Latitude;
        
        if(hash == await GetHash()) return;
        
        ModifiedDate = DateTime.UtcNow;

        if (userId.GetValueOrDefault() == Guid.Empty)
            ModifiedBy = userId;
        
        AcceptChanges();
    }
}