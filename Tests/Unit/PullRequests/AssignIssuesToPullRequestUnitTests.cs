using Application.PullRequests.Commands.IssueAssignment;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;
using Task = System.Threading.Tasks.Task;

namespace Tests.Unit.PullRequests;

public class AssignIssuesToPullRequestsUnitTests
{
    private readonly Mock<IPullRequestRepository> _pullRequestRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<IIssueRepository> _issueRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    
    public AssignIssuesToPullRequestsUnitTests()
    {
        _pullRequestRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _issueRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _notificationServiceMock = new();
    }

    [Fact]
    public async void AssignIssuesToPullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Assert
        var command = new AssignIssuesToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Guid>() { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5")}); 
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member =
            RepositoryMember.Create(user, It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        PullRequest pr = PullRequest.Create("pr", "pr", 1, repository, user.Id, It.IsAny<List<RepositoryMember>>(), It.IsAny<List<Label>>(), null, It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Issue>() { issue });

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _issueRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(issue);

        var handler = new AssignIssuesToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _issueRepositoryMock.Object, _notificationServiceMock.Object);
        
        //Act
        Guid prId = await handler.Handle(command, default);
        
        //Assert
        prId.ShouldBeOfType<Guid>();
    }


    [Fact]
    public async void AssignIssuesToPullRequest_ShouldFail_WhenPulLRequestNotFound()
    {
        //Assert
        var command = new AssignIssuesToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Guid>() { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5") });
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member =
            RepositoryMember.Create(user, It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        PullRequest pr = null;

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _issueRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(issue);

        var handler = new AssignIssuesToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _issueRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<PullRequestNotFoundException>(() => handle());
    }

    [Fact]
    public async void AssignIssuesToPullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Assert
        var command = new AssignIssuesToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Guid>() { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5") });
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member = null;
        PullRequest pr = PullRequest.Create("pr", "pr", 1, repository, user.Id, It.IsAny<List<RepositoryMember>>(), It.IsAny<List<Label>>(), null, It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Issue>() { issue });

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _issueRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(issue);

        var handler = new AssignIssuesToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _issueRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }


}