using Domain.Auth;
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

    public Task<User?> FindUserByEmail(string email)
    {
        return _context.Users
            .Include(x => x.SecondaryEmails)
            .Where(user => user.PrimaryEmail.Equals(email))
            .Where(user => !user.Deleted)
            .FirstOrDefaultAsync();
    }

    public Task<User?> FindUserById(Guid id)
    {
        return _context.Users
            .Include(x => x.SecondaryEmails)
            .Where(user => user.Id.Equals(id))
            .Where(user => !user.Deleted)
            .FirstOrDefaultAsync();
    }
}