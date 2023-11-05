using System.Runtime.Intrinsics.X86;
using Domain.Auth;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Auth.Repositories;

public class OrganizationMemberRepository: BaseRepository<OrganizationMember>, IOrganizationMemberRepository
{

    private readonly MainDbContext _context;
    public OrganizationMemberRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<OrganizationMember?> FindByUserIdAndOrganizationId(Guid userId, Guid organizationId)
    {
        return _context.OrganizationMembers
            .Where(o => o.MemberId.Equals(userId) && o.OrganizationId.Equals(organizationId))
            .FirstOrDefaultAsync();
    }

    public Task<List<Organization>> FindUserOrganizations(Guid userId)
    {
        return _context.OrganizationMembers
            .Include(x => x.Organization)
            .Where(x => x.MemberId.Equals(userId))
            .Select(x => x.Organization)
            .ToListAsync();
    }
    public Task<OrganizationMember?> FindByOrganizationIdAndRole(Guid organizationId, OrganizationMemberRole role)
    {
        return _context.OrganizationMembers
          .Where(o => o.OrganizationId.Equals(organizationId) && o.Role.Equals(role))
          .FirstOrDefaultAsync();
    }

}