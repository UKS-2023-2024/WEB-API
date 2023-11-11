using Domain.Organizations.Types;
using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationRepository: IBaseRepository<Organization>
{
    Task<Organization> FindByName(string name);
    Task<OrganizationMember?> FindMemberWithOrgPermission(PermissionParams data);
    Task<OrganizationRole?> FindRole(string name);
    Task<OrganizationMember?> FindMember(Guid organizationId, Guid memberId);
    Task<Organization?> FindById(Guid organizationId);
}