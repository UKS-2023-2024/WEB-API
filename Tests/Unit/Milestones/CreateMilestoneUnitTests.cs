using Application.Milestones.Commands.Create;
using Domain.Auth;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Milestones;

public class CreateMilestoneUnitTests
{
    private readonly Mock<IMilestoneRepository> _milestoneRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;

    public CreateMilestoneUnitTests()
    {
        _milestoneRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
    }

    [Fact]
    public async void CreateMilestone_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateMilestoneCommand("705a6c69-5b51-4156-b4cc-71e8dd111570", "first milestone",
            "description",
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), new DateOnly());
        Milestone milestone = Milestone.Create("first milestone", "description",
            new DateOnly(), Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111570"));
        RepositoryMember member =
            RepositoryMember.Create(It.IsAny<User>(), It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
            _milestoneRepositoryMock.Setup(x => x.Create(It.IsAny<Milestone>()))
                .ReturnsAsync(milestone);
            _repositoryMemberRepositoryMock
                .Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(member);
            var handler = new CreateMilestoneCommandHandler(_repositoryMemberRepositoryMock.Object,
                _milestoneRepositoryMock.Object);

            //Act
            Guid milestoneId = await handler.Handle(command, default);

            //Assert
            milestoneId.ShouldBeOfType<Guid>();
    }
    
    [Fact]
    public async void CreateMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new CreateMilestoneCommand("705a6c69-5b51-4156-b4cc-71e8dd111570", "first milestone",
            "description",
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), new DateOnly());
        Milestone milestone = Milestone.Create("first milestone", "description",
            new DateOnly(), Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111570"));
        RepositoryMember member = null;
        _milestoneRepositoryMock.Setup(x => x.Create(It.IsAny<Milestone>()))
            .ReturnsAsync(milestone);
        _repositoryMemberRepositoryMock
            .Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        var handler = new CreateMilestoneCommandHandler(_repositoryMemberRepositoryMock.Object,
            _milestoneRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}