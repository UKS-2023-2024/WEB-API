using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Auth.Repositories;

public class UserRepository: BaseRepository<User>, IUserRepository
{
    
    public UserRepository(MainDbContext context) : base(context)
    {
    }
    
}