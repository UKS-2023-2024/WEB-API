using Domain.Auth;
using Domain.Organizations.Interfaces;
using Domain.Repositories.Interfaces;
using Moq;

namespace Tests.Unit.Organizations;

public class RemoveOrganizationMemberTests
{
    private Mock<IPermissionService> _permissionServiceMock;
    private Mock<IOrganizationMemberRepository> _organizationMemberRepositoryMock;
    private Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private Mock<IOrganizationRepository> _organizationRepositoryMock;
    private Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly User _user4;

    public RemoveOrganizationMemberTests()    
    {
    }
}