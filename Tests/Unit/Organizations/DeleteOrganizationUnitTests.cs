using Application.Organizations.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Repositories.Exceptions;
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
        //Arrange 
        var user1 = User.Create("email1@gmail.com", "full name", "username", "password",
            UserRole.USER);
        var user2 = User.Create("email2@gmail.com", "full name", "username", "password",
            UserRole.USER);
       
        var organization = Organization.Create("my organization", "org@example.com", new List<User>(),user1);
        var member1 = organization.Members.First(organizationMember => organizationMember.MemberId.Equals(user1.Id));
        var member2 = organization.AddMember(user2);
        var command = new DeleteOrganizationCommand(organization.Id, user2.Id);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(user1.Id, organization.Id))
            .ReturnsAsync(member1);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(user2.Id, organization.Id))
            .ReturnsAsync(member2);

        var handler =
            new DeleteOrganizationCommandHandler(
                _organizationMemberRepository.Object, 
                _organizationRepository.Object
                );
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        await Should.ThrowAsync<MemberHasNoPrivilegeException>(() => handle());
    }
    
    [Fact]
    public async void DeleteOrganization_ShouldSucceed_WhenUserOwner()
    {
        //Arrange
        var user = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        var organization = Organization.Create("my organization", "org@example.com", new List<User>(),user);
        var member = organization.Members.First(organizationMember => organizationMember.MemberId.Equals(user.Id));
        
        var command = new DeleteOrganizationCommand(organization.Id, user.Id);
        _organizationMemberRepository.Setup(x => x.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);

        var handler =
            new DeleteOrganizationCommandHandler(
                _organizationMemberRepository.Object, 
                _organizationRepository.Object
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