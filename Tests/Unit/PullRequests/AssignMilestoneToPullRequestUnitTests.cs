﻿using Application.PullRequests.Commands.MilestoneAssignment;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;
using Task = System.Threading.Tasks.Task;

namespace Tests.Unit.PullRequests;

public class AssignMilestoneToPullRequestsUnitTests
{
    private readonly Mock<IPullRequestRepository> _pullRequestRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<IMilestoneRepository> _milestoneRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    
    public AssignMilestoneToPullRequestsUnitTests()
    {
        _pullRequestRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _milestoneRepositoryMock = new();
        _repositoryRepositoryMock = new();
    }

    [Fact]
    public async void AssignMilestoneToPullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Assert
        var command = new AssignMilestoneToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()); 
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user);;
        Milestone milestone = Milestone.Create("ms", "ms", DateOnly.FromDateTime(DateTime.Now), repository.Id);
        RepositoryMember member =
            RepositoryMember.Create(user, It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        PullRequest pr = PullRequest.Create("pr", "pr", 1, repository, user.Id, It.IsAny<List<RepositoryMember>>(), It.IsAny<List<Label>>(), null, It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Issue>());

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _milestoneRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(milestone);

        var handler = new AssignMilestoneToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _milestoneRepositoryMock.Object);
        
        //Act
        Guid prId = await handler.Handle(command, default);
        
        //Assert
        prId.ShouldBeOfType<Guid>();
    }


    [Fact]
    public async void AssignMilestoneToPullRequest_ShouldFail_WhenPulLRequestNotFound()
    {
        //Assert
        var command = new AssignMilestoneToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>());
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
        Milestone milestone = Milestone.Create("ms", "ms", DateOnly.FromDateTime(DateTime.Now), repository.Id);
        RepositoryMember member =
            RepositoryMember.Create(user, It.IsAny<Repository>(), RepositoryMemberRole.OWNER);
        PullRequest pr = null;

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _milestoneRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(milestone);

        var handler = new AssignMilestoneToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _milestoneRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<PullRequestNotFoundException>(() => handle());
    }

    [Fact]
    public async void AssignMilestoneToPullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Assert
        var command = new AssignMilestoneToPullRequestCommand(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>());
        User user = User.Create(It.IsAny<Guid>(), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
        Milestone milestone = Milestone.Create("ms", "ms", DateOnly.FromDateTime(DateTime.Now), repository.Id);
        RepositoryMember member = null;
        PullRequest pr = PullRequest.Create("pr", "pr", 1, repository, user.Id, It.IsAny<List<RepositoryMember>>(), It.IsAny<List<Label>>(), null, It.IsAny<Guid>(), It.IsAny<Guid>(), new List<Issue>());

        _pullRequestRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(pr);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>()))
            .Returns(repository);
        _milestoneRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(milestone);

        var handler = new AssignMilestoneToPullRequestCommandHandler(_pullRequestRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _milestoneRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }


}