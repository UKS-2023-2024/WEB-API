using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryRepository: BaseRepository<Repository>, IRepositoryRepository
{
    private readonly MainDbContext _context;
    public RepositoryRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public override Repository? Find(Guid id)
    {
        return _context.Repositories
            .Include(x => x.Organization)
            .Include(x => x.Members)
            .Include(x => x.PendingMembers)
            .Include(x=>x.StarredBy)
            .FirstOrDefault(x => x.Id == id);
    }
    public async Task<Repository?> FindByNameAndOwnerId(string name, Guid ownerId)
    {
        return await _context.Repositories
            .Include(r => r.Members)
            .Where(r => r.Name.ToLower().Equals(name.ToLower()) && r.Members.Any(m => (m.Member.Id == ownerId && m.Role == RepositoryMemberRole.OWNER)))
            .FirstOrDefaultAsync();
    }

    public async Task<Repository?> FindByNameAndOrganizationId(string name, Guid organizationId)
    {
        return await _context.Repositories
            .Include(r => r.Organization)
            .Where(r => r.Name.ToLower().Equals(name.ToLower()) && r.Organization.Id == organizationId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Repository>> FindAllByOwnerId(Guid id)
    {
        return _context.Repositories
            .Include(r => r.Members)
            .ThenInclude(m => m.Member)
            .Include(r => r.Organization)
            .Where(r => r.Organization == null && r.Members.Any(m => m.Member.Id == id && m.Role == RepositoryMemberRole.OWNER))
            .ToList();
    }

    public async Task<IEnumerable<Repository>> FindAllByOrganizationId(Guid id)
    {
        return _context.Repositories
            .Include(r => r.Members)
             .ThenInclude(m => m.Member)
            .Include(r => r.Organization)
            .Where(r => r.Organization.Id == id)
            .ToList();
    }
}