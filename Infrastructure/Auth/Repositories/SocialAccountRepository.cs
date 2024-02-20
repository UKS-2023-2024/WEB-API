using Domain.Auth;
using Domain.Auth.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.Repositories;

public class SocialAccountRepository : BaseRepository<SocialAccount>, ISocialAccountRepository
{
    private readonly MainDbContext _context;
    public SocialAccountRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }
}