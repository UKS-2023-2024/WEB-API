using Application.Milestones.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;
using Task = System.Threading.Tasks.Task;

namespace Tests.Unit.Milestones;

public class DeleteMilestoneUnitTests
{
    private readonly Mock<IMilestoneRepository> _milestoneRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IIssueRepository> _issueRepositoryMock;


    public DeleteMilestoneUnitTests()
    {
        _milestoneRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _issueRepositoryMock = new();
    }

    [Fact]
    public async void DeleteMilestone_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new DeleteMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Milestone milestone = Milestone.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"), "title", "description", new DateOnly(), new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"));
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        RepositoryMember repositoryMember = RepositoryMember.Create(user, repository, RepositoryMemberRole.OWNER);

        _milestoneRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(milestone);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);
        _milestoneRepositoryMock.Setup(x => x.Delete(It.IsAny<Milestone>()));
        _issueRepositoryMock.Setup(x => x.FindMilestoneIssues(It.IsAny<Guid>())).ReturnsAsync(new List<Issue>());

        
        var handler = new DeleteMilestoneCommandHandler(_repositoryMemberRepositoryMock.Object,
            _milestoneRepositoryMock.Object, _repositoryRepositoryMock.Object, _issueRepositoryMock.Object);

        //Act
        Guid milestoneId = await handler.Handle(command, default);

        //Assert
        milestoneId.ShouldBeOfType<Guid>();
    }
    
    [Fact]
    public async void DeleteMilestone_ShouldFail_WhenMilestoneIdIsWrong()
    {
        //Arrange
        var command = new DeleteMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Milestone milestone = null;
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        RepositoryMember repositoryMember = RepositoryMember.Create(user, repository, RepositoryMemberRole.OWNER);

        _milestoneRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(milestone);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);
        _milestoneRepositoryMock.Setup(x => x.Delete(It.IsAny<Milestone>()));
        _issueRepositoryMock.Setup(x => x.FindMilestoneIssues(It.IsAny<Guid>())).ReturnsAsync(new List<Issue>());

        
        var handler = new DeleteMilestoneCommandHandler(_repositoryMemberRepositoryMock.Object,
            _milestoneRepositoryMock.Object, _repositoryRepositoryMock.Object, _issueRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<MilestoneNotFoundException>(() => handle());
    }
    
    [Fact]
    public async void DeleteMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new DeleteMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Milestone milestone = Milestone.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"), "title", "description", new DateOnly(), new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"));
        Repository repository = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user);
        RepositoryMember repositoryMember = null;

        _milestoneRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(milestone);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(repositoryMember);
        _milestoneRepositoryMock.Setup(x => x.Delete(It.IsAny<Milestone>()));
        _issueRepositoryMock.Setup(x => x.FindMilestoneIssues(It.IsAny<Guid>())).ReturnsAsync(new List<Issue>());

        
        var handler = new DeleteMilestoneCommandHandler(_repositoryMemberRepositoryMock.Object,
            _milestoneRepositoryMock.Object, _repositoryRepositoryMock.Object, _issueRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}