using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Services;
using Domain.Organizations.Types;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class PermissionServiceUnitTests
{

    private readonly Mock<IOrganizationRepository> _organizationRepository;

    public PermissionServiceUnitTests()
    {
        _organizationRepository = new Mock<IOrganizationRepository>();
    }

    [Fact]
    async Task PermissionServiceThrows_ShouldFail_WhenPermissionIsWrong()
    {
        //Arange
        PermissionParams @params = new PermissionParams
        {
            Authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1"),
            Permission = "owner",
            OrganizationId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1")
        };

        _organizationRepository.Setup(
            o => o.FindMemberWithOrgPermission(
                It.IsAny<PermissionParams>()
            )
        );
        IPermissionService permissionService = new PermissionService(_organizationRepository.Object);
        
        //Act

        Func<Task> action = () => permissionService.ThrowIfNoPermission(@params);

        //Assert 
        await Should.ThrowAsync<PermissionDeniedException>(action);
    }
    
    [Fact]
    async Task PermissionServiceThrows_ShouldPass_WhenPermissionIsCorrect()
    {
        //Arange
        PermissionParams @params = new PermissionParams
        {
            Authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1"),
            Permission = "owner",
            OrganizationId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1")
        };
        OrganizationMember member = new OrganizationMember();

        _organizationRepository.Setup(
            o => o.FindMemberWithOrgPermission(
                It.IsAny<PermissionParams>()
            )
        ).ReturnsAsync(member);
        IPermissionService permissionService = new PermissionService(_organizationRepository.Object);
        
        //Act
        Func<Task> action = () => permissionService.ThrowIfNoPermission(@params);

        //Assert 
        await Should.NotThrowAsync(action);
    }

}