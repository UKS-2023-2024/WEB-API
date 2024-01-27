using Domain.Organizations.Types;
using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationRepository: IBaseRepository<Organization>
{
    Task<Organization> FindByName(string name);
    Task<Organization?> FindById(Guid organizationId);
    Task<IEnumerable<Organization>> FindAllByOwnerId(Guid id);
}