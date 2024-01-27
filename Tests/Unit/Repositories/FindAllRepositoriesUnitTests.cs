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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Application.Repositories.Queries.FindAllByOrganizationId;
using Domain.Organizations;
using Application.Repositories.Queries.FindAllRepositoriesUserBelongsTo;

namespace Tests.Unit.Repositories
{
    public class FindAllRepositoriesUnitTests
    {
        private Mock<IRepositoryRepository> _repositoryRepositoryMock;

        public FindAllRepositoriesUnitTests()
        {
            _repositoryRepositoryMock = new();
        }

        [Fact]
        async Task FindAllRepositoriesByOwnerId_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllRepositoriesByOwnerIdQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
            User user = User.Create(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, user);
            Repository repository2 = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a9"), "repository", "test", false, null, user);
            List<Repository> list = new List<Repository>() { repository, repository2 };
            _repositoryRepositoryMock.Setup(x => x.FindAllByOwnerId(It.IsAny<Guid>())).ReturnsAsync(list);

            var handler = new FindAllRepositoriesByOwnerIdQueryHandler(_repositoryRepositoryMock.Object);

            //Act
            var repositories = await handler.Handle(query, default);

            //Assert
            repositories.ShouldNotBeEmpty();
        }

        [Fact]
        async Task FindAllRepositoriesByOrganizationId_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllRepositoriesByOrganizationIdQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
            User user = User.Create(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Organization org = Organization.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "organization", "email@gmail.com", new(),user);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, org, user);
            Repository repository2 = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a9"), "repository", "test", false, org, user);
            List<Repository> list = new List<Repository>() { repository, repository2 };
            _repositoryRepositoryMock.Setup(x => x.FindAllByOrganizationId(It.IsAny<Guid>())).ReturnsAsync(list);

            var handler = new FindAllRepositoriesByOrganizationIdQueryHandler(_repositoryRepositoryMock.Object);

            //Act
            var repositories = await handler.Handle(query, default);

            //Assert
            repositories.ShouldNotBeEmpty();
        }

        [Fact]
        async Task FindAllRepositoriesUserBelongsTo_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllRepositoriesUserBelongsToQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
            User user = User.Create(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, user);
            Repository repository2 = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a9"), "repository", "test", false, null, user);
            List<Repository> list = new List<Repository>() { repository, repository2 };
            _repositoryRepositoryMock.Setup(x => x.FindAllUserBelongsTo(It.IsAny<Guid>())).ReturnsAsync(list);

            var handler = new FindAllRepositoriesUserBelongsToQueryHandler(_repositoryRepositoryMock.Object);

            //Act
            var repositories = await handler.Handle(query, default);

            //Assert
            repositories.ShouldNotBeEmpty();
        }
    }
}
