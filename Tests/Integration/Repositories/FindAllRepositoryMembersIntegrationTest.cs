using Application.Repositories.Queries.FindAllRepositoryMembers;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class FindAllRepositoryMembersIntegrationTest:BaseIntegrationTest
{
    public FindAllRepositoryMembersIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Handle_ShouldReturn2_WhenUserMemberAndOwnerHasPrivileges()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            new Guid("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5"));

        //Act
        async Task<IEnumerable<RepositoryMember>> Handle() => await _sender.Send(command);
        

        //Assert
        var res = await Handle();
        res.Count().ShouldBe(2);
    }
        
    [Fact]
    public async void Handle_ShouldFail_WhenRepositoryNotFound()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            new Guid("8e9b1323-35d3-4bf2-9f2c-9e00a21d94a5"));

        //Act
        async Task<IEnumerable<RepositoryMember>> Handle() => await _sender.Send(command);
        

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldFail_WhenUserNotMemberOfPrivateRepository()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"),
            new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"));

        //Act
        async Task<IEnumerable<RepositoryMember>> Handle() => await _sender.Send(command);
        

        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldFail_WhenUserMemberOfPrivateRepositoryButDeleted()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"),
            new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"));

        //Act
        async Task<IEnumerable<RepositoryMember>> Handle() => await _sender.Send(command);
        

        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturn1_WhenUserNotMemberButIsInOrganization()
    {
        //Arrange
        var command = new FindAllRepositoryMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"),
            new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"));

        //Act
        async Task<IEnumerable<RepositoryMember>> Handle() => await _sender.Send(command);
        

        //Assert
        var res = await Handle();
        res.Count().ShouldBe(1);
    }
}