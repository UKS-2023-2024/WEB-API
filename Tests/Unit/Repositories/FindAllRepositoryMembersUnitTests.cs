using Application.Repositories.Queries.FindAllRepositoryMembers;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class FindAllRepositoryMembersUnitTests
{
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly User _user4;
    private readonly User _user5;
    private readonly Repository _repository1;
    private readonly Repository _repository2;

    public FindAllRepositoryMembersUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user4 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211"), "dusan.janosevicasd@gmail.com", "full name", "username4", "password", UserRole.USER);
        _user5 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213"), "dusan.janosevicasd@gmail.com", "full name", "username4", "password", UserRole.USER);

        var organization = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"orgName", "dusanjanosevic007@gmail.com",
            new List<User>());

        var orgMember1 = OrganizationMember.Create(_user2.Id, organization.Id, OrganizationRole.Create("some", "some"));
        var orgMember2 = OrganizationMember.Create(_user1.Id, organization.Id, OrganizationRole.Create("some", "some"));
        var orgMember3 = OrganizationMember.Create(_user3.Id, organization.Id, OrganizationRole.Create("some", "some"));
        var orgMember4 = OrganizationMember.Create(_user5.Id, organization.Id, OrganizationRole.Create("some", "some"));
        organization.AddMember(orgMember1);
        organization.AddMember(orgMember2);
        organization.AddMember(orgMember3);
        organization.AddMember(orgMember4);

        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository1", "test", true, null, _user1);
        OverrideId(_repository1.Members.FirstOrDefault(), new Guid("8e9b1223-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repository2 = Repository.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository2", "test", true, organization, _user1);
        OverrideId(_repository2.Members.FirstOrDefault(), new Guid("8e9b1321-ffaa-4bf2-9f2c-5e00a21d92a9"));
        var member1 = _repository2.AddMember(_user2);
        OverrideId(member1, new Guid("8e9b1111-ffaa-4bf2-9f2c-5e00a21d92a9"));
        var member2 = _repository2.AddMember(_user3);
        OverrideId(member2, new Guid("8e9b1122-ffaa-4bf2-9f2c-5e00a21d92a9"));
        member2.Delete();
        _repository1.AddMember(_user4);

        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
        _repositoryRepository.Setup(x => x.Find(_repository2.Id)).Returns(_repository2);
        _repositoryMemberRepositoryMock.Setup(x => x.FindRepositoryMembers(_repository1.Id)).Returns(_repository1.Members.Where(rm=> !rm.Deleted));
        _repositoryMemberRepositoryMock.Setup(x => x.FindRepositoryMembers(_repository2.Id)).Returns(_repository2.Members.Where(rm=> !rm.Deleted));
    }
    
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
    
    [Fact]
    public void Handle_ShouldReturn2_WhenUserMemberAndOwnerHasPrivileges()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(_user1.Id,
            _repository2.Id);

        //Act
        var result = new FindAllRepositoryMembersQueryHandler(_repositoryRepository.Object,_repositoryMemberRepositoryMock.Object)
            .Handle(command,default);

        //Assert
        result.Result.Count().ShouldBe(2);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(_user1.Id,
            new Guid("8e9b1aaa-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        var handler = new FindAllRepositoryMembersQueryHandler(_repositoryRepository.Object,_repositoryMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserNotMemberOfPrivateRepository()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(_user2.Id,
            _repository1.Id);
        
        var handler = new FindAllRepositoryMembersQueryHandler(_repositoryRepository.Object,_repositoryMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserMemberOfPrivateRepositoryButDeleted()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(_user3.Id,
            _repository1.Id);
        
        var handler = new FindAllRepositoryMembersQueryHandler(_repositoryRepository.Object,_repositoryMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public void Handle_ShouldReturn2_WhenUserNotMemberButIsInOrganization()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(_user5.Id,
            _repository2.Id);

        //Act
        var result = new FindAllRepositoryMembersQueryHandler(_repositoryRepository.Object,_repositoryMemberRepositoryMock.Object)
            .Handle(command,default);

        //Assert
        result.Result.Count().ShouldBe(2);
    }
    
}