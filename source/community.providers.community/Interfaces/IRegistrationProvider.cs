using community.common.Exceptions;
using community.common.Interfaces;
using community.models.Requests.Registration;
using community.models.Responses.Authentication;
using community.models.Responses.Base;

namespace community.providers.community.Interfaces;

/// <summary>
///     Encapsulates the publicly available endpoints required to register communities and users.
/// </summary>
public interface IRegistrationProvider : IProvider
{
    /// <summary>
    ///     Encapsulates all logic required for validating requests and ultimately setting up a new community.
    /// </summary>
    /// <param name="request">The registration object containing the community and admin info.</param>
    /// <returns>On successful registration, the user is logged in and a <see cref="LoginResponse" /> is returned.</returns>
    /// <exception cref="ValidationException">Throws a validation exception if required properties are not verified.</exception>
    /// <exception cref="BusinessRuleException">Throws a BusinessRuleException if a community or admin already exists.</exception>
    Task<SingleResponse<LoginResponse>> RegisterCommunityAsync(RegisterCommunityRequest request);
}