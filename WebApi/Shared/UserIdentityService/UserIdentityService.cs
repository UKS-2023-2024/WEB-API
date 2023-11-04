using System.Security.Claims;
using Domain.Auth;
using Domain.Auth.Interfaces;

namespace WEB_API.Shared.UserIdentityService;

public class UserIdentityService: IUserIdentityService
{
    private readonly IUserRepository _userRepository;
    public UserIdentityService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public string? FindUserIdentity(ClaimsPrincipal user)
    {
        if (user.Identity is not ClaimsIdentity identity) return null;
        
        var userClaims = identity.Claims;
        IEnumerable<Claim> enumerable = userClaims as Claim[] ?? userClaims.ToArray();
        var idString = enumerable.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
        return idString;
    }

    public async Task<User> FindUserFromToken(ClaimsPrincipal user)
    {
        string id = FindUserIdentity(user);
        if (id == null) return null;
        return await _userRepository.FindUserById(new Guid(id));
    }
}