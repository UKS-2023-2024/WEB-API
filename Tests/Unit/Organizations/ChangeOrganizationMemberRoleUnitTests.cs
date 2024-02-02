using Application.Organizations.Commands.ChangeOrganizationMemberRole;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Repositories.Exceptions;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class ChangeOrganizationMemberRoleUnitTests
{
    private readonly Mock<IOrganizationMemberRepository> _organizationMemberRepositoryMock = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly Organization _organization1;
    private readonly Organization _organization2;
    private readonly OrganizationMember _organizationMember1;
    private readonly OrganizationMember _organizationMember2;
    private readonly OrganizationMember _organizationMember3;
    private readonly OrganizationMember _organizationMember4;
    private readonly OrganizationMember _organizationMember5;
    

    public ChangeOrganizationMemberRoleUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username3", "password", UserRole.USER);
        
        _organization1 = Organization.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"),"organizacija1",
            "email@gmail.com",new List<User>(), _user1);
        _organizationMember1 = _organization1.Members.First(member => member.MemberId.Equals(_user1.Id));
        _organizationMember1 = OverrideOrganizationId(_organizationMember1, _organization1.Id);
        _organizationMember2 = _organization1.AddMember(_user2);
        _organizationMember2.SetRole(OrganizationMemberRole.MODERATOR);
        _organizationMember5 = _organization1.AddMember(_user3);
        
        
        _organization2 = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"organizacija2",
            "email@gmail.com",new List<User>(), _user1);
        _organizationMember3 = _organization2.Members.First(member => member.MemberId.Equals(_user1.Id));
        _organizationMember3 = OverrideOrganizationId(_organizationMember3, _organization1.Id);
        _organizationMember4 = _organization2.AddMember(_user2);
        _organizationMember4.SetRole(OrganizationMemberRole.MEMBER);

        _organizationMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember1.MemberId,
                _organizationMember1.OrganizationId)).ReturnsAsync(_organizationMember1);
        _organizationMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember2.MemberId,
                _organizationMember2.OrganizationId)).ReturnsAsync(_organizationMember2);
        _organizationMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember3.MemberId,
                _organizationMember3.OrganizationId)).ReturnsAsync(_organizationMember3);
        _organizationMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember4.MemberId,
                _organizationMember4.OrganizationId)).ReturnsAsync(_organizationMember4);
        _organizationMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember5.MemberId,
                _organizationMember5.OrganizationId)).ReturnsAsync(_organizationMember5);
    }
    
        
    [Fact]
    public async void Handle_ShouldReturnError_WhenSenderNotOrganizationMember()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(new Guid("8e9b1cc6-35d3-4bf2-9f2c-5e00a21d92a8"),
            _organizationMember2.MemberId, _organization1.Id,OrganizationMemberRole.MODERATOR);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenSenderDoesNotHavePrivileges()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(_organizationMember4.MemberId,
            _organizationMember3.MemberId, _organization2.Id,OrganizationMemberRole.MODERATOR);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<MemberHasNoPrivilegeException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenMemberToChangeNotMember()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(_organizationMember1.MemberId,
            new Guid("8e9b1cc6-35d3-4bf2-9f2c-5e00a21d92a8"), _organization1.Id,OrganizationMemberRole.MODERATOR);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenMemberSameAsSender()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(_organizationMember2.MemberId,
            _organizationMember2.MemberId, _organization1.Id,OrganizationMemberRole.MODERATOR);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<MemberCantChangeHimselfException>(Handle);
    }
    [Fact]
    public async void Handle_ShouldReturnError_WhenSenderWantToChangeOwner1()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(_organizationMember2.MemberId,
            _organizationMember5.MemberId, _organization1.Id,OrganizationMemberRole.OWNER);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<CantChangeOwnerException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenSenderWantToChangeOwner2()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(_organizationMember2.MemberId,
            _organizationMember1.MemberId, _organization1.Id,OrganizationMemberRole.MEMBER);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<CantChangeOwnerException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnSuccess_WhenSenderHasPrivilegesAndUserValid()
    {
        //Arrange
        var command = new ChangeOrganizationMemberRoleCommand(_organizationMember1.MemberId,
            _organizationMember2.MemberId, _organization1.Id,OrganizationMemberRole.MEMBER);
        var handler = new ChangeOrganizationMemberRoleCommandHandler(_organizationMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    private OrganizationMember OverrideOrganizationId(OrganizationMember obj, Guid id)
    {
        var propertyInfo = typeof(OrganizationMember).GetProperty("OrganizationId");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
}