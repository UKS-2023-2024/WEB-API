using Application.Organizations.Commands.RemoveOrganizationMember;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class RemoveOrganizationMemberUnitTests
{
    private readonly Mock<IPermissionService> _permissionServiceMock = new();
    private Mock<IOrganizationMemberRepository> _organizationMemberRepositoryMock = new();
    private Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock = new();
    private Mock<IOrganizationRepository> _organizationRepositoryMock = new();
    private Mock<IRepositoryRepository> _repositoryRepositoryMock = new();
    private readonly Mock<IOrganizationRoleRepository> _organizationRoleRepositoryMock = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly Organization _organization1;
    private readonly Organization _organization2;
    private readonly Repository _repository1;
    private readonly OrganizationMember _organizationMember1;
    private readonly OrganizationMember _organizationMember2;
    private readonly OrganizationMember _organizationMember3;
    private readonly OrganizationMember _organizationMember4;
    private readonly OrganizationMember _organizationMember5;

    public RemoveOrganizationMemberUnitTests()    
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username3", "password", UserRole.USER);
        _permissionServiceMock.Setup(o => 
            o.ThrowIfNoPermission(It.IsAny<PermissionParams>()));
        
        _organization1 = Organization.Create("org1", "usperEmail1@gmail.com",new List<User>());
        OverrideId<Organization>(_organization1, new Guid("111b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"));
        _organization2 = Organization.Create("org2", "usperEmail2@gmail.com",new List<User>());
        OverrideId<Organization>(_organization1, new Guid("222b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"));
        
        _organizationMember1 = _organization1.AddMember(_user1, OrganizationRole.Owner());
        OverrideId<OrganizationMember>(_organizationMember1, new Guid("8e9b1cc0-1111-4bf2-9f2c-5e00a21d92a8"));
        _organizationMember2 = _organization1.AddMember(_user2, OrganizationRole.Member());
        OverrideId<OrganizationMember>(_organizationMember2, new Guid("8e9b1cc0-2222-4bf2-9f2c-5e00a21d92a8"));
        _organizationMember3 = _organization2.AddMember(_user1, OrganizationRole.Owner());
        OverrideId<OrganizationMember>(_organizationMember3, new Guid("8e9b1cc0-3333-4bf2-9f2c-5e00a21d92a8"));
        _organizationMember4 = _organization2.AddMember(_user2, OrganizationRole.Member());
        OverrideId<OrganizationMember>(_organizationMember4, new Guid("8e9b1cc0-4444-4bf2-9f2c-5e00a21d92a8"));
        _organizationMember5 = _organization2.AddMember(_user3, OrganizationRole.Member());
        OverrideId<OrganizationMember>(_organizationMember5, new Guid("8e9b1cc0-6666-4bf2-9f2c-5e00a21d92a8"));
        _organization2.RemoveMember(_organizationMember5);
        
        _repository1 = Repository.Create("super repo","koliko jak repo", false, _organization2, _user1);
        OverrideId<Repository>(_repository1, new Guid("8e9b1cc0-abab-1111-9f2c-5e00a21d92a8"));
        var repositoryMember = _repository1.AddMember(_user2);
        OverrideId<RepositoryMember>(repositoryMember, new Guid("8e9b1cc0-abab-3434-9f2c-5e00a21d92a8"));

        _organizationRoleRepositoryMock.Setup(x => x.FindByName("OWNER")).ReturnsAsync(OrganizationRole.Owner());
        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember1.MemberId,_organization1.Id))
            .ReturnsAsync(_organizationMember1);
        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember2.MemberId,_organization1.Id))
            .ReturnsAsync(_organizationMember2);
        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember3.MemberId,_organization2.Id))
            .ReturnsAsync(_organizationMember3);
        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember4.MemberId,_organization2.Id))
            .ReturnsAsync(_organizationMember4);
        _organizationMemberRepositoryMock.Setup(x => x.FindByUserIdAndOrganizationId(_organizationMember5.MemberId,_organization2.Id))
            .ReturnsAsync(_organizationMember5);
        _organizationRepositoryMock.Setup(x => x.FindById(_organization1.Id)).ReturnsAsync(_organization1);
        _organizationRepositoryMock.Setup(x => x.FindById(_organization2.Id)).ReturnsAsync(_organization2);
        _repositoryRepositoryMock
            .Setup(x => x.FindOrganizationRepositoriesThatContainsUser(_user2.Id, _organization2.Id))
            .ReturnsAsync(new List<Repository>() { _repository1 });
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id, _repository1.Id))
            .ReturnsAsync(repositoryMember);
    }
    
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserToRemoveNotInOrganization()
    {
        //Arrange
        var command = new RemoveOrganizationMemberCommand(_user1.Id,
            new Guid("8e9b1cc0-abab-1111-bbbb-5e00a21d92a8"), _organization1.Id);
        var handler = new RemoveOrganizationMemberCommandHandler(_organizationMemberRepositoryMock.Object,
                _permissionServiceMock.Object,_organizationRepositoryMock.Object,_repositoryRepositoryMock.Object,
                _repositoryMemberRepositoryMock.Object,_organizationRoleRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserToRemoveIsOwner()
    {
        //Arrange
        var command = new RemoveOrganizationMemberCommand(_user1.Id,
            _organizationMember1.MemberId, _organization1.Id);
        var handler = new RemoveOrganizationMemberCommandHandler(_organizationMemberRepositoryMock.Object,
            _permissionServiceMock.Object,_organizationRepositoryMock.Object,_repositoryRepositoryMock.Object,
            _repositoryMemberRepositoryMock.Object,_organizationRoleRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<CantRemoveOrganizationOwnerException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOrganizationDoesNotExist()
    {
        //Arrange
        var command = new RemoveOrganizationMemberCommand(_user1.Id,
            _organizationMember2.MemberId, new Guid("8e9b1cc0-abab-1111-bbbb-5e00a21d92a8"));
        var handler = new RemoveOrganizationMemberCommandHandler(_organizationMemberRepositoryMock.Object,
            _permissionServiceMock.Object,_organizationRepositoryMock.Object,_repositoryRepositoryMock.Object,
            _repositoryMemberRepositoryMock.Object,_organizationRoleRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOrganizationMemberNotInChosenOrganization()
    {
        //Arrange
        var command = new RemoveOrganizationMemberCommand(_user1.Id,
            _organizationMember5.MemberId, _organization1.Id);
        var handler = new RemoveOrganizationMemberCommandHandler(_organizationMemberRepositoryMock.Object,
            _permissionServiceMock.Object,_organizationRepositoryMock.Object,_repositoryRepositoryMock.Object,
            _repositoryMemberRepositoryMock.Object,_organizationRoleRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnSuccess_WhenUserMemberOfOrganization()
    {
        //Arrange
        var command = new RemoveOrganizationMemberCommand(_user1.Id,
            _organizationMember2.MemberId, _organization1.Id);

        //Act
        var result = new RemoveOrganizationMemberCommandHandler(_organizationMemberRepositoryMock.Object,
            _permissionServiceMock.Object,_organizationRepositoryMock.Object,_repositoryRepositoryMock.Object,
            _repositoryMemberRepositoryMock.Object,_organizationRoleRepositoryMock.Object).Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnSuccess_WhenUserMemberOfOrganizationAndMemberOfRepository()
    {
        //Arrange
        var command = new RemoveOrganizationMemberCommand(_user1.Id,
            _organizationMember4.MemberId, _organization2.Id);

        //Act
        var result = new RemoveOrganizationMemberCommandHandler(_organizationMemberRepositoryMock.Object,
            _permissionServiceMock.Object,_organizationRepositoryMock.Object,_repositoryRepositoryMock.Object,
            _repositoryMemberRepositoryMock.Object,_organizationRoleRepositoryMock.Object).Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
}