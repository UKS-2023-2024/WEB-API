using System.Reflection;
using Application.Repositories.Commands.HandleRepositoryMembers.ChangeRole;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class ChangeRepositoryMemberRoleUnitTests
{
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly User _user4;
    private readonly Repository _repository1;
    private readonly Repository _repository2;
    private readonly RepositoryMember _repoMember1;
    private readonly RepositoryMember _repoMember2;
    private readonly RepositoryMember _repoMember3;
    private readonly RepositoryMember _repoMember4;
    private readonly RepositoryMember _repoMember5;

    public ChangeRepositoryMemberRoleUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username3", "password", UserRole.USER);
        _user4 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211"), "dusan.janosevicehh@gmail.com", "full name", "username4", "password", UserRole.USER);

        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository1", "test", false, null,_user1);
        _repository2 = Repository.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository2", "test", false, null,_user1);
        _repoMember1 = RepositoryMember.Create(_user1, _repository1, RepositoryMemberRole.OWNER);
        OverrideId(_repoMember1, new Guid("8e9b1111-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember2 = RepositoryMember.Create(_user4, _repository1, RepositoryMemberRole.ADMIN);
        OverrideId(_repoMember2, new Guid("8e9b1111-ffbb-4bf2-9f2c-5e00a21d92a9"));
        _repoMember3 = RepositoryMember.Create(_user2, _repository1, RepositoryMemberRole.CONTRIBUTOR);
        OverrideId(_repoMember3, new Guid("8e9b3333-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember4 = RepositoryMember.Create(_user3, _repository2, RepositoryMemberRole.CONTRIBUTOR);
        OverrideId(_repoMember4, new Guid("8e9b4444-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember4.Delete();
        _repoMember5 = RepositoryMember.Create(_user1, _repository2, RepositoryMemberRole.OWNER);
        OverrideId(_repoMember5, new Guid("8e9b1188-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        OverrideMemberList(_repository1, new List<RepositoryMember>{_repoMember1,_repoMember3});
        OverrideMemberList(_repository2, new List<RepositoryMember>{_repoMember4,_repoMember4});

        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
        _repositoryRepository.Setup(x => x.Find(_repository2.Id)).Returns(_repository2);

        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository1.Id)).ReturnsAsync(_repoMember1);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user4.Id,_repository1.Id)).ReturnsAsync(_repoMember2);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id,_repository1.Id)).ReturnsAsync(_repoMember3);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user3.Id,_repository2.Id)).ReturnsAsync(_repoMember4);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository2.Id)).ReturnsAsync(_repoMember5);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(_repoMember1.Id)).Returns(_repoMember1);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(_repoMember2.Id)).Returns(_repoMember2);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(_repoMember3.Id)).Returns(_repoMember3);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(_repoMember4.Id)).Returns(_repoMember4);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(_repoMember5.Id)).Returns(_repoMember5);
        _repositoryMemberRepositoryMock.Setup(x => x.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(_repository1.Id,_repoMember1.Id)).Returns(0);
        _repositoryMemberRepositoryMock.Setup(x => x.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(_repository1.Id,_repoMember2.Id)).Returns(1);
        _repositoryMemberRepositoryMock.Setup(x => x.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(_repository1.Id,_repoMember3.Id)).Returns(1);
        _repositoryMemberRepositoryMock.Setup(x => x.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(_repository2.Id,_repoMember5.Id)).Returns(0);
        _repositoryMemberRepositoryMock.Setup(x => x.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(_repository2.Id,_repoMember4.Id)).Returns(1);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByRepositoryMemberIdAndRepositoryId(_repoMember1.Id,_repository1.Id)).ReturnsAsync(_repoMember1);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByRepositoryMemberIdAndRepositoryId(_repoMember3.Id,_repository1.Id)).ReturnsAsync(_repoMember3);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByRepositoryMemberIdAndRepositoryId(_repoMember4.Id,_repository2.Id)).ReturnsAsync(_repoMember4);
        
    }
    
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
    
    private Repository OverrideMemberList(Repository repo, List<RepositoryMember> members)
    {
        var propertyInfo = typeof(Repository).GetField("_members", BindingFlags.NonPublic | BindingFlags.Instance);
        if (propertyInfo == null) return repo;
        propertyInfo.SetValue(repo, members);
        return repo;
    }
    
     [Fact]
    public void Handle_ShouldReturnSuccess_WhenUserMemberAndOwnerHasPrivileges()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user1.Id,
            _repoMember3.Id, _repository1.Id,RepositoryMemberRole.ADMIN);

        //Act
        var result = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object)
            .Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserMemberAndOwnerHasNoPrivileges()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user2.Id,
            _repoMember1.Id, _repository1.Id,RepositoryMemberRole.ADMIN);
        var handler = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<MemberHasNoPrivilegeException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenNumberOfOwnersIs1()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user4.Id,
            _repoMember1.Id, _repository1.Id,RepositoryMemberRole.CONTRIBUTOR);
        var handler = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberCantBeChangedException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOwnerTriesToDeleteHimself()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user1.Id,
            _repoMember1.Id, _repository1.Id,RepositoryMemberRole.CONTRIBUTOR);
        var handler = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<MemberCantChangeHimselfException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOwnerNotFound()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user3.Id,
            _repoMember1.Id, _repository1.Id,RepositoryMemberRole.ADMIN);
        var handler = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenMemberNotFound()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user1.Id,
            new Guid("8e9b2233-ffaa-4bf2-9f2c-5e00a21d92a9"),
            _repository1.Id,RepositoryMemberRole.ADMIN);
        var handler = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenMemberFoundButDeleted()
    {
        //Arrange
        var command = new ChangeMemberRoleCommand(_user1.Id,
            _repoMember4.Id, _repository2.Id,RepositoryMemberRole.ADMIN);
        var handler = new ChangeMemberRoleCommandHandler(_repositoryMemberRepositoryMock.Object,_repositoryRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
}