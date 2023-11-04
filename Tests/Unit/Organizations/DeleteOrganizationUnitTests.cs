using Application.Organizations.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class DeleteOrganizationUnitTests
{
    private readonly Mock<IOrganizationMemberRepository> _organizationMemberRepository;
    private readonly Mock<IOrganizationRepository> _organizationRepository;

    public DeleteOrganizationUnitTests()
    {
        _organizationMemberRepository = new();
        _organizationRepository = new();
    }

    [Fact]
    public async void DeleteOrganization_ShouldFail_WhenUserNotOwner()
    {
        User user = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        OrganizationMember member = OrganizationMember.Create(user, organization, OrganizationMemberRole.CONTRIBUTOR);
        var command = new DeleteOrganizationCommand(user.Id, user);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);

        var handler =
            new DeleteOrganizationCommandHandler(_organizationMemberRepository.Object, _organizationRepository.Object);
        
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<UnauthorizedAccessException>(() => handle());
    }
    
    [Fact]
    public async void DeleteOrganization_ShouldFail_WhenUserNull()
    {
        User user = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        OrganizationMember member = null;
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        var command = new DeleteOrganizationCommand(user.Id, user);
        var handler =
            new DeleteOrganizationCommandHandler(_organizationMemberRepository.Object, _organizationRepository.Object);
        
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<UnauthorizedAccessException>(() => handle());
    }
    
    [Fact]
    public async void DeleteOrganization_ShouldSucceed_WhenUserOwner()
    {
        User user = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        OrganizationMember member = OrganizationMember.Create(user, organization, OrganizationMemberRole.OWNER);
        var command = new DeleteOrganizationCommand(user.Id, user);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);

        var handler =
            new DeleteOrganizationCommandHandler(_organizationMemberRepository.Object, _organizationRepository.Object);

        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        await Should.NotThrowAsync(() => handle());
    }
}