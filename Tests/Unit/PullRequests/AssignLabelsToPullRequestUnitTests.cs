using Application.PullRequests.Commands.IssueAssignment;
using Application.PullRequests.Commands.LabelAssignment;
using Application.PullRequests.Commands.UserAssignment;
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

public class AssignLabelsToPullRequestsUnitTests
{
    private readonly Mock<IPullRequestRepository> _pullRequestRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<ILabelRepository> _labelRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    
    public AssignLabelsToPullRequestsUnitTests()
    {
        _pullRequestRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _labelRepositoryMock = new();
        _notificationServiceMock = new();
    }

    [Fact]
    public async void AssignLabelsToPullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Assert
        var command = new AssignLabelsToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Guid>() { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5")}); 
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Label label = Label.Create("label", "label", "red", repository.Id, false);
        RepositoryMember member =
            RepositoryMember.Create(user, It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        PullRequest pr = PullRequest.Create("pr", "pr", 1, repository, user.Id, new List<RepositoryMember>() { member }, new List<Label>() { label }, null, It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Issue>());

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(member);
        _labelRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(label);

        var handler = new AssignLabelsToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _labelRepositoryMock.Object, _notificationServiceMock.Object);
        
        //Act
        Guid prId = await handler.Handle(command, default);
        
        //Assert
        prId.ShouldBeOfType<Guid>();
    }


    [Fact]
    public async void AssignLabelsToPullRequest_ShouldFail_WhenPulLRequestNotFound()
    {
        //Assert
        var command = new AssignLabelsToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Guid>() { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5") });
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
        Label label = Label.Create("label", "label", "red", repository.Id, false);
        RepositoryMember member =
            RepositoryMember.Create(user, It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        PullRequest pr = null;

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(member);

        var handler = new AssignLabelsToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _labelRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<PullRequestNotFoundException>(() => handle());
    }

    [Fact]
    public async void AssignLabelsToPullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Assert
        var command = new AssignLabelsToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Guid>() { new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5") });
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
        Label label = Label.Create("label", "label", "red", repository.Id, false);
        RepositoryMember member = null;
        PullRequest pr = PullRequest.Create("pr", "pr", 1, repository, user.Id, It.IsAny<List<RepositoryMember>>(), It.IsAny<List<Label>>(), null, It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Issue>());

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(member);

        var handler = new AssignLabelsToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _labelRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }


}