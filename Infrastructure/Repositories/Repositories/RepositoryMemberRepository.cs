using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryMemberRepository: BaseRepository<RepositoryMember>, IRepositoryMemberRepository
{

    private readonly MainDbContext _context;
    public RepositoryMemberRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }
    
    public Task<RepositoryMember?> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId)
    {
        return _context.RepositoryMembers
            .Where(o => o.Member.Id.Equals(userId) && o.RepositoryId.Equals(repositoryId))
            .FirstOrDefaultAsync();
    }

}