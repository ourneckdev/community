using System.Net.Security;
using community.common.BaseClasses;
using community.common.Definitions;
using community.common.Enumerations;
using community.common.Exceptions;
using community.common.Extensions;
using community.data.entities;
using community.data.entities.Search;
using community.data.postgres.Interfaces;
using community.models.BusinessObjects.DomainModels;
using community.models.Requests.Registration;
using community.models.Responses.Authentication;
using community.models.Responses.Base;
using community.providers.auth.Interfaces;
using community.providers.common.HttpClients;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TimeZone = community.data.entities.Locales.TimeZone;

namespace community.providers.community.Implementation;

/// <summary>
///     Provider methods used for registering communities and users.
/// </summary>
/// <param name="communityRepository">repository methods for manipulating community records.</param>
/// <param name="localeRepository">repository methods for retrieving timezone info.</param>
/// <param name="addressRepository">repository methods for manipulating address data.</param>
/// <param name="userRepository">repository methods for manipulating user data.</param>
/// <param name="contactRepository">repository methods for manipulating contact data.</param>
/// <param name="tokenProvider">provider responsible for authorizing users.</param>
/// <param name="googleRestClient">google specific http client for retrieving geolocaiton data.</param>
/// <param name="logger">the logger</param>
/// <param name="contextAccessor">access to the http context for retrieving user and header info.</param>
public class RegistrationProvider(
    ICommunityRepository communityRepository,
    ILocaleRepository localeRepository,
    IAddressRepository addressRepository,
    IUserRepository userRepository,
    IContactRepository contactRepository,
    ITokenProvider tokenProvider,
    IGoogleRestClient googleRestClient,
    ILogger<RegistrationProvider> logger,
    IHttpContextAccessor contextAccessor)
    : BaseProvider(contextAccessor), IRegistrationProvider
{
    /// <inheritdoc />
    public async Task<SingleResponse<LoginResponse>> RegisterCommunityAsync(RegisterCommunityRequest request)
    {
        var response = await MeasureExecutionAsync(async () =>
        {
            request.Validate();
            var existingCommunities = await communityRepository.FindCommunityAsync(
                new FindCommunityRecord(
                    request.Name,
                    request.Address?.AddressLine1,
                    request.Address?.City,
                    request.Address?.StateCode,
                    request.Address?.PostalCode,
                    request.PhoneNumber?.Value.FormatPhoneNumber()
                ));
            if (existingCommunities.Any())
            {
                var error = new ValidationException(ValidationMessages.PotentialDuplicateCommunity);
                foreach (var community in existingCommunities)
                {
                    if(!error.Errors.ContainsKey($"{community.Id}"))
                        error.AddError($"{community.Id}", @$"Community ""{community.Name}"" already exists.");
                    
                    if (community.AddressId.HasValue)
                        error.AddError($"{community.Id}", "A community with a matching address exists.");
                   
                    if (community.ContactId.HasValue)
                        error.AddError($"{community.Id}", $"A community with a matching phone number exists.");
                }
                throw error;
            }

            List<TimeZone> timeZones = new();
            if (request.Address?.CountryCode != null || request.Admin.Address?.CountryCode != null)
                timeZones = (await localeRepository.ListTimeZonesAsync((request.Address?.CountryCode ??
                                                                        request.Admin.Address?.CountryCode) ?? "US"))
                    .ToList();

            // if an admin is supplied, let's check to make sure they're not already setup in the system. 
            try
            {
                var userId = await userRepository.UserExistsAsync(request.Admin.Username);
                var user = (UserModel)await userRepository.GetAsync(userId);

                await user.ApplyChanges(request.Admin, UserId.GetValueOrDefault());
            }
            catch (NotFoundException)
            {
                // silently capture because this is the acceptable outcome for registering a community 
                // this means the admin has not been set up yet.
            }

            var communityId = await communityRepository.AddAsync(request.ToEntity());

            if (communityId == Guid.Empty)
                throw new BusinessRuleException(ErrorCodes.RegistrationError_Community);

            request.SetId(communityId);

            if (request.Address != null)
            {
                var geoData = await googleRestClient.GetGecodeForAddressAsync(request.Address.ToString());
                if (geoData != null)
                    request.Address.SetGeoCode(geoData.Results.FirstOrDefault());
                request.Address.TimeZoneOffset =
                    timeZones.FirstOrDefault(tz => tz.Name == request.Address.TimeZone)?.UtcOffset;

                await addressRepository.AddCommunityAddress(request.Address.ToCommunityAddress());
            }

            if (request.PhoneNumber != null)
                await contactRepository.AddAsync(request.PhoneNumber.ToContact(EntityType.Community));

            var newUserId = await userRepository.AddAsync(request.Admin.ToEntity(communityId));
            request.Admin.SetId(newUserId);

            if (request.Admin.Address != null)
            {
                var geoData = await googleRestClient.GetGecodeForAddressAsync(request.Admin.Address.ToString());
                if (geoData != null)
                    request.Admin.Address.SetGeoCode(geoData.Results.FirstOrDefault());

                request.Admin.Address.TimeZoneOffset =
                    timeZones.FirstOrDefault(tz => tz.Name == request.Admin.Address.TimeZone)?.UtcOffset;

                await addressRepository.AddUserAddressAsync(request.Admin.Address.ToUserAddress());
            }

            if (request.Admin.PhoneNumber != null)
            {
                await contactRepository.AddAsync(request.Admin.PhoneNumber.ToContact(EntityType.User));
            }
            else
            {
                if (request.Admin.Username.IsValidPhoneNumber())
                    await contactRepository.AddAsync(new Contact
                    {
                        UserId = newUserId,
                        CommunityId = communityId,
                        ContactMethodId = ContactMethods.Values
                            .FirstOrDefault(v => v.Value.Item1 == Strings.ContactMethod_MobilePhone).Key,
                        Value = request.Admin.Username,
                        EntityType = EntityType.User,
                        CanContact = true,
                        Visible = false,
                        ModifiedBy = newUserId
                    });

                if (request.Admin.Username.IsValidEmailAddress())
                    await contactRepository.AddAsync(new Contact
                    {
                        UserId = newUserId,
                        CommunityId = communityId,
                        ContactMethodId = ContactMethods.Values
                            .FirstOrDefault(v => v.Value.Item1 == Strings.ContactMethod_PersonalEmail).Key,
                        Value = request.Admin.Username,
                        EntityType = EntityType.User,
                        CanContact = true,
                        Visible = false,
                        ModifiedBy = newUserId
                    });
            }

            var newUser = await userRepository.GetAsync(newUserId);

            logger.LogInformation("Community Registration Successful: {{ \n" +
                                  @$"\t""community id"": ""{communityId}"",\n" +
                                  @$"\t""user id"": ""{newUserId}"",\n" +
                                  @$"\t""log time"": ""{DateTime.UtcNow}"",\n" +
                                  "}}");

            var loginResponse = tokenProvider.GenerateTokens(newUser);

            return new SingleResponse<LoginResponse>(loginResponse);
        });
        PrepareInformationLog(nameof(RegisterCommunityAsync), response.ExecutionMilliseconds);
        return response;
    }
}