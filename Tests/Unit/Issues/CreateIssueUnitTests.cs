using Application.Issues.Commands.Create;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;
using Task = System.Threading.Tasks.Task;

namespace Tests.Unit.Issues;

public class CreateIssueUnitTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IIssueRepository> _issueRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<ILabelRepository> _labelRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;

    public CreateIssueUnitTests()
    {
        _taskRepositoryMock = new();
        _issueRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _userRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _labelRepositoryMock = new();
        _notificationServiceMock = new();
    }

    [Fact]
    public async void CreateIssue_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateIssueCommand(Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111570"), "first issue",
            "description",
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), new List<string>(), new List<string>(), null);
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member =
            RepositoryMember.Create(It.IsAny<User>(), It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        _repositoryMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _taskRepositoryMock.Setup(x => x.GetTaskNumber())
            .ReturnsAsync(1);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(user);
        _issueRepositoryMock.Setup(x => x.Create(It.IsAny<Issue>()))
            .ReturnsAsync(issue);
        var handler = new CreateIssueCommandHandler(_repositoryMemberRepositoryMock.Object, _taskRepositoryMock.Object, _repositoryRepositoryMock.Object,
            _issueRepositoryMock.Object, _userRepositoryMock.Object, _labelRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Guid issueId = await handler.Handle(command, default);

        //Assert
        issueId.ShouldBeOfType<Guid>();
    }
    
    
    [Fact]
    public async void CreateMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new CreateIssueCommand(Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111572"), "first issue",
            "description",
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), new List<string>(), new List<string>(), null);
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member = null;
        _repositoryMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _taskRepositoryMock.Setup(x => x.GetTaskNumber())
            .ReturnsAsync(1);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(user);
        _issueRepositoryMock.Setup(x => x.Create(It.IsAny<Issue>()))
            .ReturnsAsync(issue);
        var handler = new CreateIssueCommandHandler(_repositoryMemberRepositoryMock.Object, _taskRepositoryMock.Object,
            _repositoryRepositoryMock.Object,
            _issueRepositoryMock.Object, _userRepositoryMock.Object, _labelRepositoryMock.Object, _notificationServiceMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}