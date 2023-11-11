using System.Security.Claims;
using Domain.Auth;

namespace WEB_API.Shared.UserIdentityService;

public interface IUserIdentityService
{
    string FindUserIdentity(ClaimsPrincipal user);
    Task<User?> FindUserFromToken(ClaimsPrincipal user);

}