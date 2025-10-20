using community.common.Interfaces;
using community.models.Responses.Base;
using community.models.Responses.Lookups;

namespace community.providers.lookups.Interfaces;

/// <summary>
/// Provider used for accessing lookup data.
/// </summary>
public interface ILookupProvider : IProvider
{
    /// <summary>
    /// Retrieves the active address types, including any community specific 
    /// </summary>
    /// <returns></returns>
    ValueTask<LookupResponse<AddressTypeResponse>> ListAddressTypesAsync();
    
    /// <summary>
    /// Retrieves the active contact methods, including any community specific 
    /// </summary>
    /// <returns></returns>
    ValueTask<LookupResponse<ContactMethodResponse>> ListContactMethodsAsync();
    
    /// <summary>
    /// Retrieves the available report types 
    /// </summary>
    /// <returns></returns>
    ValueTask<LookupResponse<ReportTypeResponse>> ListReportTypesAsync();
    
    /// <summary>
    /// Retrieves the available user types
    /// </summary>
    /// <returns></returns>
    ValueTask<LookupResponse<UserTypeResponse>> ListUserTypesAsync();
    
    /// <summary>
    /// Lists the available units for parcels (lot) sizes
    /// </summary>
    ValueTask<LookupResponse<ParcelSizeUnitResponse>> ListParcelSizeUnitsAsync();
}