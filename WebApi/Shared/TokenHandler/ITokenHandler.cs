using Domain.Auth;

namespace WEB_API.Shared.TokenHandler;

public interface ITokenHandler
{
    public string Generate(User user);
}