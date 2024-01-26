using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
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
}