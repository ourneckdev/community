using community.common.BaseClasses;
using community.common.Interfaces;

namespace community.data.postgres.Interfaces;

/// <summary>
///     Defines methods available to the lookup repository, for retrieving values meant to display in selection lists
/// </summary>
public interface ILookupRepository : IRepository
{
    // /// <summary>
    // /// List common and custom address types
    // /// </summary>
    // /// <param name="communityId">Optional community id allows retrieval of custom types per community.</param>
    // /// <returns></returns>
    // ValueTask<IEnumerable<AddressType>> ListAddressTypesAsync(Guid? communityId);
    //
    // /// <summary>
    // /// Lists common and customer report types
    // /// </summary>
    // /// <param name="communityId">Optional community id allows retrieval of custom types per community.</param>
    // /// <returns></returns>
    // ValueTask<IEnumerable<ContactMethod>> ListContactMethodsAsync(Guid? communityId);
    //
    // /// <summary>
    // /// Lists common and customer report types
    // /// </summary>
    // /// <returns></returns>
    // ValueTask<IEnumerable<ReportType>> ListReportTypesAsync();
    //
    // /// <summary>
    // /// Lists the system user types.
    // /// </summary>
    // /// <returns></returns>
    // ValueTask<IEnumerable<UserType>> ListUserTypesAsync();
    //
    // /// <summary>
    // /// Lists the units for parcel sizes.
    // /// </summary>
    // /// <returns></returns>
    // ValueTask<IEnumerable<ParcelSizeUnit>> ListParcelSizeUnitsAsync();

    /// <summary>
    ///     Lists types that implememnt a commmunity id.
    /// </summary>
    /// <param name="communityId">Optional community id</param>
    /// <returns></returns>
    ValueTask<IEnumerable<T>> ListAsync<T>(Guid? communityId = null) where T : BaseLookupEntity;
}