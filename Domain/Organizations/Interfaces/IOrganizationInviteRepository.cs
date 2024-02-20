using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationInviteRepository: IBaseRepository<OrganizationInvite>
{
    Task<OrganizationInvite?> FindById(Guid id);
    OrganizationInvite? FindByOrgAndMember(Guid organizationId, Guid memberId);
}