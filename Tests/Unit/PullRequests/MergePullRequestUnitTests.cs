using System.Reflection;
using Application.PullRequests.Commands.Merge;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Branches;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;
using Task = System.Threading.Tasks.Task;

namespace Tests.Unit.PullRequests;

public class MergePullRequestUnitTests
{
    private readonly Mock<IPullRequestRepository> _pullRequestRepository = new();
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepository = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly Mock<IGitService> _gitService = new();
    private readonly Mock<INotificationService> _notificationService = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly RepositoryMember _repoMember1; 
    private readonly RepositoryMember _repoMember2; 
    private readonly Repository _repository1;
    private readonly PullRequest _pullRequest1;
    private readonly PullRequest _pullRequest2;
    private readonly PullRequest _pullRequest3;

    public MergePullRequestUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username3", "password", UserRole.USER);
        
        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository1", "test", true, null,_user1);
        _repoMember1 = RepositoryMember.Create(_user1, _repository1, RepositoryMemberRole.OWNER);
        OverrideId(_repoMember1, new Guid("8e9b1111-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember2 = RepositoryMember.Create(_user2, _repository1, RepositoryMemberRole.WRITE);
        OverrideId(_repoMember2, new Guid("8e9b3333-ffaa-4bf2-9f2c-5e00a21d92a9"));
        OverrideMemberList(_repository1, new List<RepositoryMember>{_repoMember1,_repoMember2});
        
        var branch1 = Branch.Create("branch1", _repository1.Id, false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        _pullRequest1 = PullRequest.Create("pr1", "pull request1", 0, _repository1, new Guid(),
            new List<RepositoryMember>(), new List<Label>(), null, new Guid(), new Guid(),new List<Issue>());
        _pullRequest1 = OverrideId(_pullRequest1, new Guid("8e9b1cc1-ffaa-4bf2-1111-5e00a21d92a9"));
        _pullRequest1 = OverrideFromBranch(_pullRequest1, branch1);
        _pullRequest1 = OverrideToBranch(_pullRequest1, branch1);
        
        _pullRequest2 = PullRequest.Create("pr2", "pull request2", 0, _repository1, new Guid(),
            new List<RepositoryMember>(), new List<Label>(), null, new Guid(), new Guid(),new List<Issue>());
        _pullRequest2 = OverrideId(_pullRequest2, new Guid("8e9b1cc1-ffaa-4bf2-2222-5e00a21d92a9"));
        _pullRequest2 = OverrideFromBranch(_pullRequest2, branch1);
        _pullRequest2 = OverrideToBranch(_pullRequest2, branch1);
        _pullRequest2.ClosePullRequest(_user1.Id);
        
        _pullRequest3 = PullRequest.Create("pr3", "pull request3", 0, _repository1, new Guid(),
            new List<RepositoryMember>(), new List<Label>(), null, new Guid(), new Guid(),new List<Issue>());
        _pullRequest3 = OverrideId(_pullRequest3, new Guid("8e9b1cc1-ffaa-4bf2-3333-5e00a21d92a9"));
        _pullRequest3 = OverrideFromBranch(_pullRequest3, branch1);
        _pullRequest3 = OverrideToBranch(_pullRequest3, branch1);
        _pullRequest3.MergePullRequest(_user1.Id);

        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
        
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository1.Id)).ReturnsAsync(_repoMember1);
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id,_repository1.Id)).ReturnsAsync(_repoMember2);

        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository1.Id)).ReturnsAsync(_repoMember1);
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id,_repository1.Id)).ReturnsAsync(_repoMember2);
        
        _pullRequestRepository.Setup(x => x.FindByIdAndRepositoryId(_pullRequest1.RepositoryId, _pullRequest1.Id))
            .ReturnsAsync(_pullRequest1);
        _pullRequestRepository.Setup(x => x.FindByIdAndRepositoryId(_pullRequest2.RepositoryId, _pullRequest2.Id))
            .ReturnsAsync(_pullRequest2);
        _pullRequestRepository.Setup(x => x.FindByIdAndRepositoryId(_pullRequest3.RepositoryId, _pullRequest3.Id))
            .ReturnsAsync(_pullRequest3);
    }
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
    
    private PullRequest OverrideFromBranch(PullRequest obj, Branch branch)
    {
        var propertyInfo = typeof(PullRequest).GetProperty("FromBranch");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, branch);
        return obj;
    }
    private PullRequest OverrideToBranch(PullRequest obj, Branch branch)
    {
        var propertyInfo = typeof(PullRequest).GetProperty("ToBranch");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, branch);
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
    public void Handle_ShouldReturnSuccess_WhenMergeCorrect()
    {
        //Arrange
        var command = new MergePullRequestCommand(_pullRequest1.Id,_repository1.Id,_user1.Id,MergeType.MERGE);

        //Act
        var result = new MergePullRequestCommandHandler(_repositoryRepository.Object, _pullRequestRepository.Object,
                _repositoryMemberRepository.Object, _gitService.Object,_notificationService.Object)
            .Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var command = new MergePullRequestCommand(_pullRequest1.Id,new Guid(),_user1.Id,MergeType.MERGE);

        //Act
        var handler = new MergePullRequestCommandHandler(_repositoryRepository.Object, _pullRequestRepository.Object,
                _repositoryMemberRepository.Object, _gitService.Object,_notificationService.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryMemberNotFound()
    {
        //Arrange
        var command = new MergePullRequestCommand(_pullRequest1.Id,_repository1.Id,new Guid(),MergeType.MERGE);

        //Act
        var handler = new MergePullRequestCommandHandler(_repositoryRepository.Object, _pullRequestRepository.Object,
            _repositoryMemberRepository.Object, _gitService.Object,_notificationService.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    [Fact]
    public async void Handle_ShouldReturnError_WhenPullRequestNotFound()
    {
        //Arrange
        var command = new MergePullRequestCommand(new Guid(),_repository1.Id,_user1.Id,MergeType.MERGE);

        //Act
        var handler = new MergePullRequestCommandHandler(_repositoryRepository.Object, _pullRequestRepository.Object,
            _repositoryMemberRepository.Object, _gitService.Object,_notificationService.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<PullRequestNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenPullRequestClosed()
    {
        //Arrange
        var command = new MergePullRequestCommand(_pullRequest2.Id,_repository1.Id,_user1.Id,MergeType.MERGE);

        //Act
        var handler = new MergePullRequestCommandHandler(_repositoryRepository.Object, _pullRequestRepository.Object,
            _repositoryMemberRepository.Object, _gitService.Object,_notificationService.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<PullRequestClosedException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenPullRequestAlreadyMerged()
    {
        //Arrange
        var command = new MergePullRequestCommand(_pullRequest3.Id,_repository1.Id,_user1.Id,MergeType.MERGE);

        //Act
        var handler = new MergePullRequestCommandHandler(_repositoryRepository.Object, _pullRequestRepository.Object,
            _repositoryMemberRepository.Object, _gitService.Object,_notificationService.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<PullRequestMergedException>(Handle);
    }
}