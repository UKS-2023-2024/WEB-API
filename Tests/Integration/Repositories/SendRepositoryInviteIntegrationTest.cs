using Application.Repositories.Commands.HandleRepositoryMembers.SendRepositoryInvite;
using Domain.Auth.Exceptions;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class SendRepositoryInviteIntegrationTest: BaseIntegrationTest
{
    public SendRepositoryInviteIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task SendInvite_PersonalRepository_ShouldPass()
    {
    
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
        var invite = _context.RepositoryInvites.FirstOrDefault(o =>
            o.RepositoryId.Equals(repository.Id) && o.UserId.Equals(userId));
        invite.ShouldNotBeNull();
    }
    [Fact]
    public async Task SendInvite_OrganizationRepository_ShouldPass()
    {
    
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo2"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
        var invite = _context.RepositoryInvites.FirstOrDefault(o =>
            o.RepositoryId.Equals(repository.Id) && o.UserId.Equals(userId));
        invite.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task SendInvite_OrganizationRepository_ShouldPassIfUserDeleted()
    {
    
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo2"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
        var invite = _context.RepositoryInvites.FirstOrDefault(o =>
            o.RepositoryId.Equals(repository.Id) && o.UserId.Equals(userId));
        invite.ShouldNotBeNull();
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserAlreadyMember()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);
    
        //Assert
        await Should.ThrowAsync<AlreadyRepositoryMemberException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOwnerNotAnOwner()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);
    
        //Assert
        await Should.ThrowAsync<MemberNotOwnerException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOwnerNotExists()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);
    
        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserToAddNotExists()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213");
        
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);
    
        //Assert
        await Should.ThrowAsync<UserNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserNotAMemberAndUserNotInOrganization()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var userId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo2"));
        var inviteCommand = new SendInviteCommand(ownerId, userId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(inviteCommand);
    
        //Assert
        await Should.ThrowAsync<UserNotAOrganizationMemberException>(Handle);
    }
    
}