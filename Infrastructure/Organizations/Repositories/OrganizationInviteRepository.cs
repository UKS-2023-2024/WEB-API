

using System.Data.Entity;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Organizations.Repositories;

public class OrganizationInviteRepository: BaseRepository<OrganizationInvite>, IOrganizationInviteRepository
{
    private readonly MainDbContext _context;
    public OrganizationInviteRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<OrganizationInvite> FindByToken(string token)
    {
        return _context.OrganizationInvites
            .Where(invite => invite.Token.Equals(token))
            .FirstOrDefaultAsync();
    }

    public OrganizationInvite? FindByOrgAndMember(Guid organizationId, Guid memberId)
    {
        return _context.OrganizationInvites
            .FirstOrDefault(invite => invite.OrganizationId.Equals(organizationId)
                                      && invite.MemberId.Equals(memberId));
    }
}