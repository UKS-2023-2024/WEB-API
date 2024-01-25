using Application.Issues.Commands.Enums;
using Application.Issues.Commands.Update;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;
using Task = System.Threading.Tasks.Task;

namespace Tests.Unit.Issues;

public class AssignIssueToUserUnitTests
{
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IIssueRepository> _issueRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILabelRepository> _labelRepositoryMock;
    
    public AssignIssueToUserUnitTests()
    {
        _repositoryMemberRepositoryMock = new();
        _taskRepositoryMock = new();
        _issueRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _userRepositoryMock = new();
        _labelRepositoryMock = new();
    }

    [Fact]
    public async void AssignIssueToIsser_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Assert
        var command = new UpdateIssueCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<TaskState>(), It.IsAny<int>(), It.IsAny<Guid>(),
            new List<string>(), It.IsAny<List<string>>(), UpdateIssueFlag.ASSIGNEES, It.IsAny<Guid>()); 
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member =
            RepositoryMember.Create(It.IsAny<User>(), It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(user);
        _issueRepositoryMock.Setup(x => x.FindById(It.IsAny<Guid>()))
            .ReturnsAsync(issue);
        _repositoryMemberRepositoryMock.Setup(x => x.FindAllByIdsAndRepositoryId(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<RepositoryMember>());
        var handler = new UpdateIssueCommandHandler(_repositoryMemberRepositoryMock.Object, _taskRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _issueRepositoryMock.Object,
            _userRepositoryMock.Object, _labelRepositoryMock.Object);
        
        //Act
        Guid issueId = await handler.Handle(command, default);
        
        //Assert
        issueId.ShouldBeOfType<Guid>();
    }
    
    [Fact]
    public async void AssignIssueToIsser_ShouldFail_WhenUserIsNotRepositoryMember()
    {
        //Assert
        var command = new UpdateIssueCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<TaskState>(), It.IsAny<int>(), It.IsAny<Guid>(),
            new List<string>(), It.IsAny<List<string>>(), UpdateIssueFlag.ASSIGNEES, It.IsAny<Guid>()); 
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member = null;
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(user);
        _issueRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(issue);
        _repositoryMemberRepositoryMock.Setup(x => x.FindAllByIds(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<RepositoryMember>());
        var handler = new UpdateIssueCommandHandler(_repositoryMemberRepositoryMock.Object, _taskRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _issueRepositoryMock.Object,
            _userRepositoryMock.Object, _labelRepositoryMock.Object);
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
    
     [Fact]
    public async void AssignIssueToIsser_ShouldFail_WhenUserIsNull()
    {
        //Assert
        var command = new UpdateIssueCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<TaskState>(), It.IsAny<int>(), It.IsAny<Guid>(),
            new List<string>(), It.IsAny<List<string>>(), UpdateIssueFlag.ASSIGNEES, It.IsAny<Guid>());
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        User foundUser = null;
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Issue issue = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
        RepositoryMember member =
            RepositoryMember.Create(It.IsAny<User>(), It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _userRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(foundUser);
        _issueRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(issue);
        _repositoryMemberRepositoryMock.Setup(x => x.FindAllByIdsAndRepositoryId(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<RepositoryMember>());
        var handler = new UpdateIssueCommandHandler(_repositoryMemberRepositoryMock.Object, _taskRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _issueRepositoryMock.Object,
            _userRepositoryMock.Object, _labelRepositoryMock.Object);
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<UserNotFoundException>(() => handle());
    }
}