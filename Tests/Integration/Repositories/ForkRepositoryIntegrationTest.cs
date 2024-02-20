using Application.Repositories.Commands.Fork;
using Domain.Auth.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class ForkRepositoryIntegrationTest:BaseIntegrationTest
{
    public ForkRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task ForkRepository_ShouldPass_WhenRepositoryPrivate()
    {
    
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo4"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9") && rm.RepositoryId == repository!.Id);
        var removeCommand = new ForkRepositoryCommand(ownerId,repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async Task ForkRepository_ShouldPass_WhenRepositoryPublic()
    {
    
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9") && rm.RepositoryId == repository!.Id);
        var removeCommand = new ForkRepositoryCommand(ownerId,repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async void ForkRepository_ShouldFail_WhenRepositoryPrivateAndUserNotMember()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo4"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5") && rm.RepositoryId == repository!.Id);
        var removeCommand = new ForkRepositoryCommand(ownerId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ForkRepository_ShouldFail_WhenRepositoryToForkNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var removeCommand = new ForkRepositoryCommand(ownerId, new Guid());
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ForkRepository_ShouldFail_WhenUserNotFound()
    {
        //Arrange
        var ownerId = new Guid();
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5") && rm.RepositoryId == repository!.Id);
        var removeCommand = new ForkRepositoryCommand(ownerId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<UserNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ForkRepository_ShouldFail_WhenUserAlreadyHasRepositoryWithForkRepositoryName()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo3"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5") && rm.RepositoryId == repository!.Id);
        var removeCommand = new ForkRepositoryCommand(ownerId, repository!.Id);
        
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<YouAlreadyHaveRepositoryWithThisNameException>(Handle);
    }
}