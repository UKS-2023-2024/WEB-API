using Application.Organizations.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Repositories.Exceptions;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class DeleteOrganizationUnitTests
{
    private readonly Mock<IOrganizationMemberRepository> _organizationMemberRepository;
    private readonly Mock<IOrganizationRepository> _organizationRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IGitService> _gitServiceMock;
    public DeleteOrganizationUnitTests()
    {
        _organizationMemberRepository = new();
        _organizationRepository = new();
        _userRepository = new();
        _gitServiceMock = new();
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
        
        _userRepository.Setup(x => x.FindUserById(user1.Id))
            .ReturnsAsync(user1);
        _userRepository.Setup(x => x.FindUserById(user2.Id))
            .ReturnsAsync(user2);

        var handler =
            new DeleteOrganizationCommandHandler(
                _organizationMemberRepository.Object, 
                _organizationRepository.Object,
                _gitServiceMock.Object,
                _userRepository.Object
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
        _userRepository.Setup(x => x.FindUserById(user.Id))
            .ReturnsAsync(user);

        var handler =
            new DeleteOrganizationCommandHandler(
                _organizationMemberRepository.Object, 
                _organizationRepository.Object,
                _gitServiceMock.Object,
                _userRepository.Object
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