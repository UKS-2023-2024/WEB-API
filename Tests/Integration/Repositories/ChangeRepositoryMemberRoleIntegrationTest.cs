using Application.Repositories.Commands.HandleRepositoryMembers.ChangeRole;
using Domain.Organizations.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class ChangeRepositoryMemberRoleIntegrationTest : BaseIntegrationTest

{
    public ChangeRepositoryMemberRoleIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldBeSuccess_WhenUserMemberAndOwnerHasPrivileges()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.ADMIN);

        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenUserMemberAndOwnerHasNoPrivileges()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.ADMIN);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<MemberHasNoPrivilegeException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenOwnerTriesToChangeHimself()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5")  && rm.RepositoryId.Equals(repository!.Id));
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.READ);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<MemberCantChangeHimselfException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenTryingToChangeOwner()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5") && rm.RepositoryId.Equals(repository!.Id));
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.READ);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<CantChangeOwnerException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenTryingToChangeOwner2()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9") && rm.RepositoryId.Equals(repository!.Id));
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.OWNER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<CantChangeOwnerException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenOwnerNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1123-35d3-4bf2-9f2c-5e00a21d92a9");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5") && rm.RepositoryId.Equals(repository!.Id));
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.ADMIN);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenMemberNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var command = new ChangeMemberRoleCommand(ownerId,
            new Guid("7e9b1234-35d3-4bf2-9f2c-5e00a21d92a9"), repository!.Id,RepositoryMemberRole.ADMIN);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenMemberFoundButDeleted()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7")  && rm.RepositoryId.Equals(repository!.Id));
        var command = new ChangeMemberRoleCommand(ownerId,
            repoMember!.Id, repository!.Id,RepositoryMemberRole.ADMIN);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
}