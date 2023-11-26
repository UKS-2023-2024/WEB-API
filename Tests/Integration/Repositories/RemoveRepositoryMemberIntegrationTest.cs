using Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class RemoveRepositoryMemberIntegrationTest: BaseIntegrationTest
{
    public RemoveRepositoryMemberIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task RemoveRepositoryUser_ShouldPass()
    {
    
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var removeCommand = new RemoveRepositoryMemberCommand(ownerId, repoMember!.Id, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async void RemoveRepositoryUser_Fail_WhenOwnerHasNoPrivileges()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        var removeCommand = new RemoveRepositoryMemberCommand(ownerId, repoMember!.Id, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<MemberHasNoPrivilegeException>(Handle);
    }
    
    [Fact]
    public async void RemoveRepositoryUser_Fail_WhenOwnerNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1444-35d3-4bf2-9f2c-5e00a21d92a7");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        var removeCommand = new RemoveRepositoryMemberCommand(ownerId, repoMember!.Id, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void RemoveRepositoryUser_Fail_WhenMemberNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1444-35d3-4bf2-9f2c-5e00a21d92a7");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var removeCommand = new RemoveRepositoryMemberCommand(ownerId, new Guid("7e9b1444-35d3-4bf2-9f2c-5e00a21d92a7"), repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void RemoveRepositoryUser_Fail_WhenMemberFoundButDeleted()
    {
        //Arrange
        var ownerId = new Guid("7e9b1444-35d3-4bf2-9f2c-5e00a21d92a7");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"));
        var removeCommand = new RemoveRepositoryMemberCommand(ownerId, repoMember!.Id, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
}