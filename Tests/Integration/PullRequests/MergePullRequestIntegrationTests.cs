using Application.PullRequests.Commands;
using Application.PullRequests.Commands.Merge;
using Domain.Repositories.Exceptions;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.PullRequests;

[Collection("Sequential")]
public class MergePullRequestIntegrationTests : BaseIntegrationTest
{
    public MergePullRequestIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task MergePullRequest_ShouldPass()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo"));
        var pullRequest = _context.PullRequests.FirstOrDefault(o => o.Title.Equals("pr"));
        
        var command = new MergePullRequestCommand(pullRequest!.Id,repository!.Id,ownerId,MergeType.MERGE);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async Task MergePullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo"));
        var pullRequest = _context.PullRequests.FirstOrDefault(o => o.Title.Equals("pr"));
        
        var command = new MergePullRequestCommand(pullRequest!.Id,repository!.Id,ownerId,MergeType.MERGE);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async Task MergePullRequest_ShouldFail_WhenRepositoryNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213");
        var pullRequest = _context.PullRequests.FirstOrDefault(o => o.Title.Equals("pr"));
        
        var command = new MergePullRequestCommand(pullRequest!.Id,new Guid(),ownerId,MergeType.MERGE);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async Task MergePullRequest_ShouldFail_WhenPullRequestNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo"));
        
        var command = new MergePullRequestCommand(new Guid(),repository!.Id,ownerId,MergeType.MERGE);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async Task MergePullRequest_ShouldFail_WhenPullRequestClosed()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo"));
        var pullRequest = _context.PullRequests.FirstOrDefault(o => o.Title.Equals("pr2"));
        
        var command = new MergePullRequestCommand(pullRequest!.Id,repository!.Id,ownerId,MergeType.MERGE);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<PullRequestClosedException>(Handle);
    }
    
    [Fact]
    public async Task MergePullRequest_ShouldFail_WhenPullRequestAlreadyMerged()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo"));
        var pullRequest = _context.PullRequests.FirstOrDefault(o => o.Title.Equals("pr3"));
        
        var command = new MergePullRequestCommand(pullRequest!.Id,repository!.Id,ownerId,MergeType.MERGE);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<PullRequestMergedException>(Handle);
    }
}