using System.Security.Claims;

namespace WEB_API.Shared.UserIdentityService;

public class UserIdentityService: IUserIdentityService
{
    public string? FindUserIdentity(ClaimsPrincipal user)
    {
        if (user.Identity is not ClaimsIdentity identity) return null;
        
        var userClaims = identity.Claims;
        IEnumerable<Claim> enumerable = userClaims as Claim[] ?? userClaims.ToArray();
        var idString = enumerable.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
        return idString;
    }
}