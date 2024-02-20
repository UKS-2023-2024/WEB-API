using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Organizations.Repositories;

public class OrganizationRepository: BaseRepository<Organization>, IOrganizationRepository
{
    private readonly MainDbContext _context;
    public OrganizationRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Organization?> FindByName(string name)
    {
        return await _context.Organizations
            .Where(o => o.Name.Equals(name))
            .FirstOrDefaultAsync();
    }

    public Task<Organization?> FindById(Guid organizationId)
    {
        return _context.Organizations
            .Include(organization => organization.Members )
            .FirstOrDefaultAsync(org => org.Id.Equals(organizationId));
    }

    public async Task<IEnumerable<Organization>> FindAllByOwnerId(Guid id)
    {
        return _context.Organizations
            .Include(r => r.Members)
            .ThenInclude(m => m.Member)
            .Where(r => r.Members.Any(m => m.Member.Id == id && m.Role == OrganizationMemberRole.OWNER))
            .ToList();
    }
    
    public override async Task<Organization> Create(Organization organization)
    {
        organization.Created();
        await _context.Set<Organization>().AddAsync(organization);
        await _context.SaveChangesAsync();
        return organization;
    }

    public override void Update(Organization organization)
    {
        organization.Updated();
        _context.Update(organization);
        _context.SaveChanges();
    }
}