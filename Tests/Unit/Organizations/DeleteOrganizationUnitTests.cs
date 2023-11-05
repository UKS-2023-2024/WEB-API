using Application.Organizations.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Services;
using Domain.Organizations.Types;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class DeleteOrganizationUnitTests
{
    private readonly Mock<IOrganizationMemberRepository> _organizationMemberRepository;
    private readonly Mock<IOrganizationRepository> _organizationRepository;
    private readonly PermissionService _permissionService;
    public DeleteOrganizationUnitTests()
    {
        _organizationMemberRepository = new();
        _organizationRepository = new();
        _permissionService = new PermissionService(_organizationRepository.Object);
    }
    
    [Fact]
    public async void DeleteOrganization_ShouldFail_WhenUserNotOwner()
    {
        //Arrange 
        User user = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
       
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        OrganizationMember member = OrganizationMember.Create(user, organization, OrganizationRole.Owner());
        var command = new DeleteOrganizationCommand(organization.Id, user.Id);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);

        _organizationRepository.Setup(
            o => o.FindMemberWithOrgPermission(It.IsAny<PermissionParams>())
        );

        var handler =
            new DeleteOrganizationCommandHandler(
                _organizationMemberRepository.Object, 
                _organizationRepository.Object, 
                _permissionService
                );
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        await Should.ThrowAsync<PermissionDeniedException>(() => handle());
    }
    
    [Fact]
    public async void DeleteOrganization_ShouldSucceed_WhenUserOwner()
    {
        //Arange
        User user = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        var role = OrganizationRole.Create("MEMBER", "Has most of the rights ");
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        OrganizationMember member = OrganizationMember.Create(user, organization,role);
        
        var command = new DeleteOrganizationCommand(organization.Id, user.Id);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _organizationRepository.Setup(
            o => o.FindMemberWithOrgPermission(It.IsAny<PermissionParams>())
        ).ReturnsAsync(member);
        
        var handler =
            new DeleteOrganizationCommandHandler(
                _organizationMemberRepository.Object, 
                _organizationRepository.Object, 
                _permissionService
            );

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        await Should.NotThrowAsync(() => handle());
    }
}