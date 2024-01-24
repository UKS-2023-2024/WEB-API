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
using Application.Repositories.Queries.FindAllUsersWatchingRepository;

namespace Tests.Unit.Repositories
{
    public class FindAllUsersWatchingRepositoryUnitTests
    {
        private Mock<IRepositoryRepository> _repositoryRepositoryMock;
        private Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;

        public FindAllUsersWatchingRepositoryUnitTests()
        {
            _repositoryRepositoryMock = new();
            _repositoryMemberRepositoryMock = new();
        }

        [Fact]
        async Task FindAllUsersWatchingRepository_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllUsersWatchingQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
            User user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, user);
            repository.AddMember(user);
            repository.AddToWatchedBy(user);
            _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);

            var handler = new FindAllUsersWatchingQueryHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object);

            //Act
            var repositories = await handler.Handle(query, default);

            //Assert
            repositories.ShouldNotBeEmpty();
        }

    }
}
