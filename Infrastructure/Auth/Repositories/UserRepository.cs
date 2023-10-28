using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.Repositories;

public class UserRepository: BaseRepository<User>, IUserRepository
{
    private readonly MainDbContext _context;
    public UserRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<User> FindUserByEmail(string email)
    {
        return _context.Users
            .Include(x => x.SecondaryEmails)
            .Where(user => user.PrimaryEmail.Equals(email))
            .FirstOrDefaultAsync();
    }
}