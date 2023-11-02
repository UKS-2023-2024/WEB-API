using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Auth.Repositories;

public class OrganizationRepository: BaseRepository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(MainDbContext context) : base(context)
    {
    }
}