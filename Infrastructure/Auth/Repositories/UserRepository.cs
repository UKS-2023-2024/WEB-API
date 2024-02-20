using Domain.Auth;
using Domain.Auth.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
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
            .Include(x => x.SocialAccounts)
            .Where(user => user.PrimaryEmail.Equals(email))
            .Where(user => !user.Deleted)
            .FirstOrDefaultAsync();
    }

    public Task<User?> FindUserById(Guid id)
    {
        return _context.Users
            .Include(x => x.SecondaryEmails)
            .Include(x => x.SocialAccounts)
            .Where(user => user.Id.Equals(id))
            .Where(user => !user.Deleted)
            .FirstOrDefaultAsync();
    }

    public Task<User?> FindByUsername(string username)
    {
        return _context.Users
            .Where(u => u.Username.Equals(username))
            .FirstOrDefaultAsync();
    }

    public Task<List<User>> SearchUsers(String value)
    {
        return _context.Users
            .Where(x => x.FullName.Contains(value) || x.PrimaryEmail.Contains(value) || x.Username.Contains(value))
            .Take(10)
            .ToListAsync();
    }
    
    public Task<List<User>> FindByStarredRepository(Guid repositoryId)
    {
        return _context.Users
            .Where(x => x.Starred.Any(r => r.Id == repositoryId))
            .ToListAsync();
    }
    
    public override async Task<User> Create(User user)
    {
        user.Created();
        await _context.Set<User>().AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public override void Update(User user)
    {
        user.Updated();
        _context.Update(user);
        _context.SaveChanges();
    }
}