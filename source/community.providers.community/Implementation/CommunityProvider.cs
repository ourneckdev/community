using community.common.BaseClasses;
using community.providers.community.Interfaces;
using Microsoft.AspNetCore.Http;

namespace community.providers.community.Implementation;

/// <summary>
///     Exposes methods used for manipulating community data.
/// </summary>
/// <param name="contextAccessor"></param>
public class CommunityProvider(
    IHttpContextAccessor contextAccessor)
    : BaseProvider(contextAccessor), ICommunityProvider
{
}