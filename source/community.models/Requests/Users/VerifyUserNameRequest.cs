using community.models.Abstract;

namespace community.models.Requests.Users;

/// <summary>
///     Verifies the phone or email the user is attempting to register with.
/// </summary>
public record VerifyUserNameRequest(string Code) : VerifiableUserNameRecord
{
}