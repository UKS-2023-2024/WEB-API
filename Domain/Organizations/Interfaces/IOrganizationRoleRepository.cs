using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationRoleRepository: IBaseRepository<OrganizationRole>
{
    Task<OrganizationRole> FindByName(string name);
}