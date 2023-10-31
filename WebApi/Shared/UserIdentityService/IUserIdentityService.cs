using System.Security.Claims;

namespace WEB_API.Shared.UserIdentityService;

public interface IUserIdentityService
{
    string? FindUserIdentity(ClaimsPrincipal user);
}