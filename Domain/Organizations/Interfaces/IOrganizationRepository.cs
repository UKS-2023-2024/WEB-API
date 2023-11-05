using Domain.Shared.Interfaces;

namespace Domain.Organizations.Interfaces;

public interface IOrganizationRepository: IBaseRepository<Organization>
{
    Task<Organization> FindByName(string name);
}