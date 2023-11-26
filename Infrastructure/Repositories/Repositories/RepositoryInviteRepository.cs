using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryInviteRepository : BaseRepository<RepositoryInvite>, IRepositoryInviteRepository
{
    private readonly MainDbContext _context;
    public RepositoryInviteRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public RepositoryInvite? FindByRepoAndMember(Guid repositoryId, Guid memberId)
    {
        return _context.RepositoryInvites
            .FirstOrDefault(invite => invite.RepositoryId.Equals(repositoryId)
                                      && invite.UserId.Equals(memberId));
    }
    public Task<RepositoryInvite?> FindById(Guid id)
    {
        return _context.RepositoryInvites
            .Include(invite => invite.Repository)
            .FirstOrDefaultAsync(i => i.Id.Equals(id));
    }
}