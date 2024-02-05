using Domain.Shared.Interfaces;

namespace Domain.Auth.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User?> FindUserByEmail(string email);
    
    public Task<User?> FindUserById(Guid id);
    public Task<User?> FindByUsername(string username);

    public Task<List<User>> SearchUsers(String value);

    public Task<List<User>> FindByStarredRepository(Guid repositoryId);
}