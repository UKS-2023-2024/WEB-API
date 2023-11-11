using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Organizations.Repositories;

public class OrganizationRoleRepository: BaseRepository<OrganizationRole>, IOrganizationRoleRepository
{

    private readonly MainDbContext _context;
    public OrganizationRoleRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }


    public Task<OrganizationRole> FindByName(string name)
    {
        return _context.OrganizationRoles
            .FirstOrDefaultAsync(role => role.Name.Equals(name));
    }
}