using community.common.Definitions;
using community.common.Exceptions;
using community.data.entities;
using community.models.BusinessObjects.Google.Geocode;
using community.models.Requests;

namespace community.models.Abstract;

/// <summary>
///     Base address data used across requests
/// </summary>
public abstract record BaseAddressRequest(
    Guid AddressTypeId,
    string? LotNumber,
    string AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string City,
    string StateCode,
    string TimeZone,
    string PostalCode,
    string? CountyCode,
    string CountryCode = "USA")
    : IRequiresValidation
{
    /// <summary>
    ///     Overridden initializes that accepts community id.
    /// </summary>
    /// <param name="communityId"></param>
    /// <param name="addressTypeId"></param>
    /// <param name="lotNumber"></param>
    /// <param name="addressLine1"></param>
    /// <param name="addressLine2"></param>
    /// <param name="addressLine3"></param>
    /// <param name="city"></param>
    /// <param name="stateCode"></param>
    /// <param name="timeZone"></param>
    /// <param name="postalCode"></param>
    /// <param name="countyCode"></param>
    /// <param name="countryCode"></param>
    protected BaseAddressRequest(
        Guid communityId,
        Guid addressTypeId,
        string? lotNumber,
        string addressLine1,
        string? addressLine2,
        string? addressLine3,
        string city,
        string stateCode,
        string timeZone,
        string postalCode,
        string? countyCode,
        string countryCode = "USA")
        : this(addressTypeId, lotNumber, addressLine1, addressLine2, addressLine3, city, stateCode, timeZone,
            postalCode,
            countyCode, countryCode)
    {
        CommunityId = communityId;
    }

    /// <inheritdoc cref="IRequiresValidation.Validate" />
    public void Validate(ValidationException? exception = null)
    {
        var shouldThrow = exception == null;

        exception ??= new ValidationException(ValidationMessages.AddressValidationErrors);

        if (string.IsNullOrEmpty(AddressLine1))
            exception.AddError(nameof(AddressLine1), ValidationMessages.StreetAddressRequired);

        if (string.IsNullOrEmpty(City))
            exception.AddError(nameof(City), ValidationMessages.CityRequired);

        if (string.IsNullOrEmpty(StateCode))
            exception.AddError(nameof(StateCode), ValidationMessages.StateRequired);

        if (string.IsNullOrEmpty(PostalCode))
            exception.AddError(nameof(PostalCode), ValidationMessages.PostalCodeRequired);

        if (string.IsNullOrEmpty(CountryCode))
            exception.AddError(nameof(CountryCode), ValidationMessages.CountryRequired);

        if (string.IsNullOrEmpty(TimeZone))
            exception.AddError(nameof(TimeZone), ValidationMessages.TimeZoneRequired);

        if (exception.Errors.Any() && shouldThrow)
            throw exception;
    }

    /// <summary>
    ///     Uses generic conversion to convert to UserAddress, appends UserId
    /// </summary>
    /// <returns></returns>
    public UserAddress ToUserAddress()
    {
        var address = ToAddress<UserAddress>();
        address.UserId = UserId.GetValueOrDefault();
        return address;
    }

    /// <summary>
    ///     Uses generic conversion to convert to commmunity address
    /// </summary>
    /// <returns></returns>
    public CommunityAddress ToCommunityAddress()
    {
        return ToAddress<CommunityAddress>();
    }

    /// <summary>
    ///     Adds the id of the user making the update.
    /// </summary>
    /// <param name="userId"></param>
    public BaseAddressRequest AddUserAudit(Guid? userId)
    {
        ModifiedBy = userId;
        return this;
    }


    /// <summary>
    ///     Generic conversion to base address
    /// </summary>
    /// <returns></returns>
    private T ToAddress<T>() where T : BaseAddressEntity, new()
    {
        return new T
        {
            Id = Id.GetValueOrDefault(),
            CommunityId = CommunityId,
            AddressTypeId = AddressTypeId,
            LotNumber = LotNumber,
            AddressLine1 = AddressLine1,
            AddressLine2 = AddressLine2,
            AddressLine3 = AddressLine3,
            City = City,
            StateCode = StateCode,
            TimeZone = TimeZone,
            TimeZoneOffset = TimeZoneOffset.GetValueOrDefault(),
            PostalCode = PostalCode,
            CountyCode = CountyCode,
            CountryCode = CountryCode,
            Longitude = Longitude,
            Latitude = Latitude,
            PlaceId = PlaceId,
            ModifiedBy = ModifiedBy
        };
    }

    /// <summary>
    ///     Outputs the address a string to be passed to google for geocoding
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{AddressLine1} {AddressLine2 ?? " "}{City} {StateCode} {PostalCode} {CountyCode}";
    }

    /// <summary>
    ///     Back fills the geocode data if not supplied
    /// </summary>
    /// <param name="result"></param>
    public void SetGeoCode(Result? result)
    {
        var latLong = result?.GetLatLong();
        Longitude ??= latLong?.Longitude;
        Latitude ??= latLong?.Latitude;
        PlaceId ??= result?.PlaceId;
    }

    #region Properties

    /// <summary>
    /// </summary>
    public Guid? Id { get; protected init; }

    /// <summary>
    ///     The community id of the address
    /// </summary>
    public Guid CommunityId { get; set; }

    /// <summary>
    ///     The user id of a user address.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    ///     gets or sets the longitude of the requested address
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    ///     Gets or sets the Longitude of the requested address
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    ///     Gets or sets the place id from the google response.
    /// </summary>
    public string? PlaceId { get; set; }

    /// <summary>
    ///     Gets or sets the timezone offset.
    /// </summary>
    public TimeSpan? TimeZoneOffset { get; set; }

    /// <summary>
    ///     Gets or sets who's making the change.
    /// </summary>
    protected Guid? ModifiedBy { get; set; }

    #endregion
}