

using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Organizations.Repositories;

public class OrganizationInviteRepository: BaseRepository<OrganizationInvite>, IOrganizationInviteRepository
{
    private readonly MainDbContext _context;
    public OrganizationInviteRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<OrganizationInvite?> FindById(Guid id)
    {
        return _context.OrganizationInvites
            .FirstOrDefaultAsync(i => i.Id.Equals(id));
    }

    public OrganizationInvite? FindByOrgAndMember(Guid organizationId, Guid memberId)
    {
        return _context.OrganizationInvites
            .FirstOrDefault(invite => invite.OrganizationId.Equals(organizationId)
                                      && invite.UserId.Equals(memberId));
    }
}