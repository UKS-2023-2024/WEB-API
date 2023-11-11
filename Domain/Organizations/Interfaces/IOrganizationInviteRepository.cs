using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationInviteRepository: IBaseRepository<OrganizationInvite>
{
    Task<OrganizationInvite> FindByToken(string token);
    OrganizationInvite? FindByOrgAndMember(Guid organizationId, Guid memberId);
}