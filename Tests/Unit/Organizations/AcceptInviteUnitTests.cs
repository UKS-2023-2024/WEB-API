using System.Reflection;
using Application.Organizations.Commands.AcceptInvite;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class AcceptInviteUnitTests
{
    private Mock<IOrganizationRoleRepository> _organizationRoleRepository = new();
    private  Mock<IUserRepository> _userRepository = new();
    private  Mock<IOrganizationInviteRepository> _organizationInviteRepository = new();
    private  Mock<IOrganizationRepository> _organizationRepository = new();



    private OrganizationInvite OverrideDate(OrganizationInvite invite, DateTime date)
    {
        PropertyInfo propertyInfo = typeof(OrganizationInvite).GetProperty("ExpiresAt");
        if (propertyInfo == null) return invite;
        propertyInfo.SetValue(invite, date);
        return invite;
    }

    [Fact]
    async Task AcceptInvite_ShouldPass()
    {
        //Arrange
        var memberId = new Guid();
        var organizationId = new Guid();
        var authorized = new Guid();
        var user = User.Create(memberId, "test@gmail.com", "Test Test", "test123", "asdasdasd", UserRole.USER);
        var invite = OrganizationInvite.Create(memberId, organizationId);
        var role = OrganizationRole.Create("MEMBER", "");
        var organization = Organization.Create(organizationId, "org", "", new List<User>());
        
        _organizationInviteRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(invite);
        _userRepository.Setup(r => r.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        _organizationRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(organization);
        _organizationRoleRepository.Setup(r => r.FindByName(It.IsAny<string>()))
            .ReturnsAsync(role);
        _organizationRepository.Setup(r => r.Update(It.IsAny<Organization>()))
            .Verifiable();
        _organizationInviteRepository.Setup(r => r.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        var commandHandler = new AcceptInviteCommandHandler(
            _organizationRoleRepository.Object,
            _userRepository.Object,
            _organizationInviteRepository.Object,
            _organizationRepository.Object
        );
        //Act
        Func<Task> handle = async () => { await commandHandler.Handle(command, default); };
        //Assert
        await Should.NotThrowAsync(handle);
        _organizationRepository.Verify();
        _organizationInviteRepository.Verify();
    }
    
    
    [Fact]
    async Task AcceptInvite_ShouldFail_WhenInviteDoesntExist()
    {
        //Arrange
        var memberId = new Guid();
        var organizationId = new Guid();
        var authorized = new Guid();
        var user = User.Create(memberId, "test@gmail.com", "Test Test", "test123", "asdasdasd", UserRole.USER);
        var invite = OrganizationInvite.Create(memberId, organizationId);
        var role = OrganizationRole.Create("MEMBER", "");
        var organization = Organization.Create(organizationId, "org", "", new List<User>());

        _organizationInviteRepository.Setup(r => r.FindById(It.IsAny<Guid>()));
        _userRepository.Setup(r => r.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        _organizationRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(organization);
        _organizationRoleRepository.Setup(r => r.FindByName(It.IsAny<string>()))
            .ReturnsAsync(role);
        _organizationRepository.Setup(r => r.Update(It.IsAny<Organization>()))
            .Verifiable();
        _organizationInviteRepository.Setup(r => r.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        var commandHandler = new AcceptInviteCommandHandler(
            _organizationRoleRepository.Object,
            _userRepository.Object,
            _organizationInviteRepository.Object,
            _organizationRepository.Object
        );
        //Act
        Func<Task> handle = async () => { await commandHandler.Handle(command, default); };
        //Assert
        await Should.ThrowAsync<OrganizationInviteNotFoundException>(handle);
    }
    
    [Fact]
    async Task AcceptInvite_ShouldFail_WhenInviteHasExpired()
    {
        //Arrange
        var memberId = new Guid();
        var organizationId = new Guid();
        var authorized = new Guid();
        var user = User.Create(memberId, "test@gmail.com", "Test Test", "test123", "asdasdasd", UserRole.USER);
        var invite = OrganizationInvite.Create(memberId, organizationId);
        var role = OrganizationRole.Create("MEMBER", "");
        invite = OverrideDate(invite, DateTime.Now.AddDays(-1).ToUniversalTime());
        var organization = Organization.Create(organizationId, "org", "", new List<User>());
        
        _organizationInviteRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(invite);
        _userRepository.Setup(r => r.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        _organizationRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(organization);
        _organizationRoleRepository.Setup(r => r.FindByName(It.IsAny<string>()))
            .ReturnsAsync(role);
        _organizationRepository.Setup(r => r.Update(It.IsAny<Organization>()))
            .Verifiable();
        _organizationInviteRepository.Setup(r => r.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        var commandHandler = new AcceptInviteCommandHandler(
            _organizationRoleRepository.Object,
            _userRepository.Object,
            _organizationInviteRepository.Object,
            _organizationRepository.Object
        );
        //Act
        Func<Task> handle = async () => { await commandHandler.Handle(command, default); };
        //Assert
        await Should.ThrowAsync<InvitationExpiredException>(handle);
    }
    
    [Fact]
    async Task AcceptInvite_ShouldFail_WhenOrganizationDoesntExist()
    {
        //Arrange
        var memberId = new Guid();
        var organizationId = new Guid();
        var authorized = new Guid();
        var user = User.Create(memberId, "test@gmail.com", "Test Test", "test123", "asdasdasd", UserRole.USER);
        var invite = OrganizationInvite.Create(memberId, organizationId);
        var role = OrganizationRole.Create("MEMBER", "");
        var organization = Organization.Create(organizationId, "org", "", new List<User>());
        
        _organizationInviteRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(invite);
        _userRepository.Setup(r => r.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        _organizationRepository.Setup(r => r.FindById(It.IsAny<Guid>()));
        _organizationRoleRepository.Setup(r => r.FindByName(It.IsAny<string>()))
            .ReturnsAsync(role);
        _organizationRepository.Setup(r => r.Update(It.IsAny<Organization>()))
            .Verifiable();
        _organizationInviteRepository.Setup(r => r.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        var commandHandler = new AcceptInviteCommandHandler(
            _organizationRoleRepository.Object,
            _userRepository.Object,
            _organizationInviteRepository.Object,
            _organizationRepository.Object
        );
        //Act
        Func<Task> handle = async () => { await commandHandler.Handle(command, default); };
        //Assert
        await Should.ThrowAsync<OrganizationNotFoundException>(handle);
    }
    
    [Fact]
    async Task AcceptInvite_ShouldFail_WhenUserDoesntExist()
    {
        //Arrange
        var memberId = new Guid();
        var organizationId = new Guid();
        var authorized = new Guid();
        var user = User.Create(memberId, "test@gmail.com", "Test Test", "test123", "asdasdasd", UserRole.USER);
        var invite = OrganizationInvite.Create(memberId, organizationId);
        var role = OrganizationRole.Create("MEMBER", "");
        var organization = Organization.Create(organizationId, "org", "", new List<User>());
        
        _organizationInviteRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(invite);
        _userRepository.Setup(r => r.FindUserById(It.IsAny<Guid>()));
        _organizationRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(organization);
        _organizationRoleRepository.Setup(r => r.FindByName(It.IsAny<string>()))
            .ReturnsAsync(role);
        _organizationRepository.Setup(r => r.Update(It.IsAny<Organization>()))
            .Verifiable();
        _organizationInviteRepository.Setup(r => r.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        var commandHandler = new AcceptInviteCommandHandler(
            _organizationRoleRepository.Object,
            _userRepository.Object,
            _organizationInviteRepository.Object,
            _organizationRepository.Object
        );
        //Act
        Func<Task> handle = async () => { await commandHandler.Handle(command, default); };
        //Assert
        await Should.ThrowAsync<UserNotFoundException>(handle);
    }
    
    [Fact]
    async Task AcceptInvite_ShouldFail_WhenOwnerIsNotSame()
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var organizationId = Guid.NewGuid();
        var authorized = Guid.NewGuid();
        var user = User.Create(memberId, "test@gmail.com", "Test Test", "test123", "asdasdasd", UserRole.USER);
        var invite = OrganizationInvite.Create(memberId, organizationId);
        var role = OrganizationRole.Create("MEMBER", "");
        var organization = Organization.Create(organizationId, "org", "", new List<User>());
        
        _organizationInviteRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(invite);
        _userRepository.Setup(r => r.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        _organizationRepository.Setup(r => r.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(organization);
        _organizationRoleRepository.Setup(r => r.FindByName(It.IsAny<string>()))
            .ReturnsAsync(role);
        _organizationRepository.Setup(r => r.Update(It.IsAny<Organization>()))
            .Verifiable();
        _organizationInviteRepository.Setup(r => r.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        var commandHandler = new AcceptInviteCommandHandler(
            _organizationRoleRepository.Object,
            _userRepository.Object,
            _organizationInviteRepository.Object,
            _organizationRepository.Object
        );
        //Act
        Func<Task> handle = async () => { await commandHandler.Handle(command, default); };
        //Assert
        await Should.ThrowAsync<NotInviteOwnerException>(handle);
    }
}