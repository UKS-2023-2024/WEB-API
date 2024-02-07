using Application.PullRequests.Commands;
using Domain.Branches.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.PullRequests;

[Collection("Sequential")]
public class CreatePullRequestIntegrationTests: BaseIntegrationTest
{
    public CreatePullRequestIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreatePullRequest_ShouldPass()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo5"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == ownerId && rm.RepositoryId == repository!.Id);
        
        var command = new CreatePullRequestCommand(ownerId,"super title","super description",repository!.Id,
            new List<Guid>{repoMember!.Id},new List<Guid>(),null,
            new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b2"), new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"),new List<Guid>());
        
        //Act
        async Task<Guid> Handle() => await _sender.Send(command);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async Task CreatePullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo5"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == ownerId && rm.RepositoryId == repository!.Id);
        
        var command = new CreatePullRequestCommand(new Guid(),"super title","super description",repository!.Id,
            new List<Guid>{repoMember!.Id},new List<Guid>(),null,
            new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b2"), new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"),new List<Guid>());
        
        //Act
        async Task<Guid> Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async Task CreatePullRequest_ShouldFail_WhenBranchesTheSame()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo5"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == ownerId && rm.RepositoryId == repository!.Id);
        
        var command = new CreatePullRequestCommand(ownerId,"super title","super description",repository!.Id,
            new List<Guid>{repoMember!.Id},new List<Guid>(),null,
            new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"), new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"),new List<Guid>());
        
        //Act
        async Task<Guid> Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<CantCreatePullRequestOnSameBranchException>(Handle);
    }
    
    [Fact]
    public async Task CreatePullRequest_ShouldFail_WhenBranchNotFound()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var repository = _context.Repositories.FirstOrDefault(o => o.Name.Equals("repo5"));
        var repoMember =
            _context.RepositoryMembers.FirstOrDefault(rm =>
                rm.Member.Id == ownerId && rm.RepositoryId == repository!.Id);
        
        var command = new CreatePullRequestCommand(ownerId,"super title","super description",repository!.Id,
            new List<Guid>{repoMember!.Id},new List<Guid>(),null,
            new Guid(), new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"),new List<Guid>());
        
        //Act
        async Task<Guid> Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<BranchNotFoundException>(Handle);
    }
}