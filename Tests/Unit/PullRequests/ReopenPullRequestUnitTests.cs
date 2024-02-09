using Application.Milestones.Commands.Close;
using Application.PullRequests.Commands.Close;
using Application.PullRequests.Commands.Reopen;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
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


namespace Tests.Unit.PullRequests;

public class ReopenPullRequestUnitTests
{
    private readonly Mock<IPullRequestRepository> _pullRequestRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;

    public ReopenPullRequestUnitTests()
    {
        _pullRequestRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _notificationServiceMock = new();
    }

    [Fact]
    public async void ReopenPullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new ReopenPullRequestCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        var pullRequest = PullRequest.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"), "pr", "pr", 1, repository, user.Id, new List<RepositoryMember>(), new List<Label>(), null, new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b6"), new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b7"), new List<Issue>());
        pullRequest.ClosePullRequest(user.Id);
        RepositoryMember repositoryMember = RepositoryMember.Create(user, repository, RepositoryMemberRole.OWNER);

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(pullRequest);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);

        
        var handler = new ReopenPullRequestCommandHandler(_repositoryMemberRepositoryMock.Object,
            _pullRequestRepositoryMock.Object, _repositoryRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        PullRequest closedPr = await handler.Handle(command, default);

        //Assert
        closedPr.ShouldBeOfType<PullRequest>();
        closedPr.State.ShouldBeEquivalentTo(TaskState.OPEN);
    }

    [Fact]
    public async void ReopenPullRequest_ShouldFail_WhenPrIsAlreadyOpen()
    {
        //Arrange
        var command = new ReopenPullRequestCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        var pullRequest = PullRequest.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"), "pr", "pr", 1, repository, user.Id, new List<RepositoryMember>(), new List<Label>(), null, new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b6"), new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b7"), new List<Issue>());
      
        RepositoryMember repositoryMember = RepositoryMember.Create(user, repository, RepositoryMemberRole.OWNER);

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(pullRequest);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);


        var handler = new ReopenPullRequestCommandHandler(_repositoryMemberRepositoryMock.Object,
            _pullRequestRepositoryMock.Object, _repositoryRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<System.Threading.Tasks.Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<PullRequestAlreadyOpenedException>(() => handle());
    }

    [Fact]
    public async void ReopenPullRequest_ShouldFail_WhenPullRequestIdIsWrong()
    { //Arrange
        var command = new ReopenPullRequestCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("9e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        PullRequest? pullRequest = null;

        RepositoryMember repositoryMember = RepositoryMember.Create(user, repository, RepositoryMemberRole.OWNER);
        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(pullRequest);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);


        var handler = new ReopenPullRequestCommandHandler(_repositoryMemberRepositoryMock.Object,
            _pullRequestRepositoryMock.Object, _repositoryRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<System.Threading.Tasks.Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<PullRequestNotFoundException>(() => handle());
    }

    [Fact]
    public async void ReopenPullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new ReopenPullRequestCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        var pullRequest = PullRequest.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"), "pr", "pr", 1, repository, user.Id, new List<RepositoryMember>(), new List<Label>(), null, new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b6"), new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b7"), new List<Issue>());

        RepositoryMember? repositoryMember = null;
        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(pullRequest);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);


        var handler = new ReopenPullRequestCommandHandler(_repositoryMemberRepositoryMock.Object,
            _pullRequestRepositoryMock.Object, _repositoryRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<System.Threading.Tasks.Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }

}