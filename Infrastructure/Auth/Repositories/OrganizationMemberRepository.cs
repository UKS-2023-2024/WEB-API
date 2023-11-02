using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Auth.Repositories;

public class OrganizationMemberRepository: BaseRepository<OrganizationMember>, IOrganizationMemberRepository
{
    public OrganizationMemberRepository(MainDbContext context) : base(context)
    {
    }
}