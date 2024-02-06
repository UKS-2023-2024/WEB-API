using Application.Auth.Commands.Update;
using Application.Repositories.Queries.FindAllByOwnerId;
using Domain.Auth.Enums;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using Shouldly;
using Domain.Tasks.Interfaces;
using Application.Milestones.Queries.FindCompletionPercentageOfMilestone;
using Domain.Tasks.Enums;
using Domain.Tasks;

namespace Tests.Unit.Milestones
{
    public class FindMilestoneCompletionPercentageUnitTests
    {
        private Mock<IIssueRepository> _issueRepositoryMock;

        public FindMilestoneCompletionPercentageUnitTests()
        {
            _issueRepositoryMock = new();
        }

        [Fact]
        public async void FindIMilestoneCompletionPercentage_ShouldReturnZero()
        {
            //Arrange
            var query = new FindCompletionPercentageOfMilestoneQuery(It.IsAny<Guid>());
            List<Issue> issues = new()
            {
            };
            _issueRepositoryMock.Setup(x => x.FindMilestoneIssues(It.IsAny<Guid>())).ReturnsAsync(issues);

            var handler = new FindCompletionPercentageOfMilestoneQueryHandler(_issueRepositoryMock.Object);

            //Act
            var percentage = await handler.Handle(query, default);

            //Assert
            percentage.ShouldBeEquivalentTo((double)0);
        }

        [Fact]
        public async void FindIMilestoneCompletionPercentage_ShouldReturn50()
        {
            //Arrange
            var query = new FindCompletionPercentageOfMilestoneQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
            User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user); ;
            Issue issue1 = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository,
            user, new List<RepositoryMember>(), new List<Label>(), null);
            Issue issue2 = Issue.Create("second issue", "description", TaskState.CLOSED, 1, repository,
           user, new List<RepositoryMember>(), new List<Label>(), null);
            List<Issue> issues = new()
            {
                issue1,
                issue2
            };

            _issueRepositoryMock.Setup(x => x.FindMilestoneIssues(It.IsAny<Guid>())).ReturnsAsync(issues);

            var handler = new FindCompletionPercentageOfMilestoneQueryHandler(_issueRepositoryMock.Object);

            //Act
            var percentage = await handler.Handle(query, default);

            //Assert
            percentage.ShouldBeEquivalentTo((double)50);
        }


    }
}
