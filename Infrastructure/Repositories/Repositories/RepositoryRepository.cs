using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryRepository: BaseRepository<Repository>, IRepositoryRepository
{
    private readonly MainDbContext _context;
    public RepositoryRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Repository?> FindByNameAndOwner(string name, Guid ownerId)
    {
        return await _context.Repositories
            .Include(r => r.Members)
            .Where(r => r.Name.ToLower().Equals(name.ToLower()) && r.Members.Any(m => (m.Member.Id == ownerId && m.Role == RepositoryMemberRole.OWNER)))
            .FirstOrDefaultAsync();
    }

    public async Task<Repository?> FindByNameAndOrganization(string name, Guid organizationId)
    {
        return await _context.Repositories
            .Include(r => r.Organization)
            .Where(r => r.Name.ToLower().Equals(name.ToLower()) && r.Organization.Id == organizationId)
            .FirstOrDefaultAsync();
    }

}