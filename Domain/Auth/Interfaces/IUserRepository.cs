using Domain.Shared.Interfaces;

namespace Domain.Auth.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public User FindUserByEmail(string email);
}