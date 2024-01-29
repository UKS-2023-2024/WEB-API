using Domain.Auth;

namespace Domain.Shared.Interfaces;

public interface IGitService
{
    public Task CreateUser(User user, string password);
    public Task SetPublicKey(User user, string pk);
}