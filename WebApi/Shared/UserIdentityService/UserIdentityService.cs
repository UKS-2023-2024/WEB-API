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

    public Guid FindUserIdentity(ClaimsPrincipal user)
    {
        if (user.Identity is not ClaimsIdentity identity) throw new UnauthorizedAccessException();
        
        var userClaims = identity.Claims;
        IEnumerable<Claim> enumerable = userClaims as Claim[] ?? userClaims.ToArray();
        var id = enumerable.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(id)) throw new UnauthorizedAccessException();
        return Guid.Parse(id);
    }

    public Task<User?> FindUserFromToken(ClaimsPrincipal user)
    {
        var id = FindUserIdentity(user);
        return _userRepository.FindUserById(id);
    }
}