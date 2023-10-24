using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Auth.Repositories;

public class UserRepository: IUserRepository
{
    private readonly MainDbContext _context;
    
    public UserRepository(MainDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public List<User> FindAll()
    {
        return _context.Set<User>().ToList();
    }

    public void Create(User user)
    {
        _context.Set<User>().Add(user);
        _context.SaveChanges();
    }
}