using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationMemberRepository: IBaseRepository<OrganizationMember>
{
    Task<OrganizationMember> FindByUserIdAndOrganizationId(Guid userId, Guid organizationId);
}