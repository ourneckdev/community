using community.common.Definitions;
using community.common.Exceptions;
using community.data.entities;

namespace community.models.Requests.Registration;

/// <summary>
///     Encapsulates all the data required to begin setting up a new community.
/// </summary>
/// <param name="Name">The name of the community</param>
/// <param name="Description">A short description of the community</param>
/// <param name="Website">The communities main website</param>
/// <param name="NumberOfParcels"></param>
/// <param name="ParcelSize"></param>
/// <param name="ParcelSizeUnitId"></param>
/// <param name="NumberOfResidents"></param>
/// <param name="AverageHomeValue"></param>
/// <param name="Admin">Information related to the user registering the community, setup as an admin.</param>
/// <param name="Address">Any addresses associated with the community</param>
/// <param name="PhoneNumber">The main phone number for the community</param>
public record RegisterCommunityRequest(
    string Name,
    string? Description,
    string? Website,
    int? NumberOfParcels,
    decimal? ParcelSize,
    Guid? ParcelSizeUnitId,
    int? NumberOfResidents,
    decimal? AverageHomeValue,
    RegisterCommunityAdminRequest Admin,
    RegisterAddressRequest? Address,
    RegisterContactRequest? PhoneNumber)
    : IRequiresValidation
{
    /// <inheritdoc cref="IRequiresValidation.Validate" />
    public void Validate(ValidationException? validationException = null)
    {
        var shouldThrow = validationException == null;

        validationException ??= new ValidationException();

        if (string.IsNullOrEmpty(Name))
            validationException.AddError(nameof(Name), ValidationMessages.CommunityNameIsRequired);

        if (Address == null)
            validationException.AddError(nameof(Addresses), ValidationMessages.PrimaryAddressRequired);
        else
            Address.Validate(validationException);

        if (PhoneNumber == null)
            validationException.AddError(nameof(PhoneNumber), ValidationMessages.ContactMethodRequired);
        else
            PhoneNumber?.Validate(validationException);

        if (!string.IsNullOrEmpty(Website))
        {
            var validUri = Uri.TryCreate(Website, UriKind.Absolute, out var validatedUrl);
            if (!validUri)
                validationException.AddError(nameof(Website), ValidationMessages.InvalidWebAddress);

            if (validatedUrl?.Scheme != Uri.UriSchemeHttps)
                validationException.AddError(nameof(Website), ValidationMessages.InvalidWebAddressScheme);
        }

        Admin.Validate(validationException);

        if (validationException.Errors.Any() && shouldThrow)
            throw validationException;
    }

    /// <summary>
    ///     Converts a register community record to an entity in preparation for saving.
    /// </summary>
    /// <returns></returns>
    public Community ToEntity()
    {
        return new Community
        {
            Name = Name,
            Description = Description,
            Website = Website,
            NumberOfParcels = NumberOfParcels,
            ParcelSize = ParcelSize,
            ParcelSizeUnitId = ParcelSizeUnitId,
            NumberOfResidents = NumberOfResidents,
            AverageHomeValue = AverageHomeValue
        };
    }

    /// <summary>
    /// Sets the community id on child objects.
    /// </summary>
    /// <param name="id"></param>
    public void SetId(Guid id)
    {
        if(Address != null) Address.CommunityId = id;
        if(PhoneNumber != null) PhoneNumber.CommunityId = id;
        if(Admin.PhoneNumber != null) Admin.PhoneNumber.CommunityId = id;
        if(Admin.Address != null) Admin.Address.CommunityId = id;
    }
}