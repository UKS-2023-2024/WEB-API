﻿using Application.Repositories.Commands.Create;
using Domain.Auth.Enums;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Auth.Commands.Update;
using FluentResults;
using Domain.Exceptions.Repositories;

namespace Tests.Unit.Repositories
{
    public class UpdateRepositoryUnitTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRepositoryRepository> _repositoryRepositoryMock;
        private Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
        private Mock<IOrganizationRepository> _organizationRepositoryMock;

        public UpdateRepositoryUnitTests()
        {
            _userRepositoryMock = new();
            _repositoryRepositoryMock = new();
            _repositoryMemberRepositoryMock = new();
            _organizationRepositoryMock = new();
        }

        [Fact]
        public async Task UpdateRepositoryForUser_ShouldBeSuccess_WhenRepositoryNameUnique()
        {
            // Arrange
            var command = new UpdateRepositoryCommand(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test-repository", "test", false);

            User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null);
            RepositoryMember member = RepositoryMember.Create(foundUser, repository, RepositoryMemberRole.OWNER);
            _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.Find(repository.Id)).Returns(repository);
            _repositoryMemberRepositoryMock.Setup(x => x.FindRepositoryOwner(repository.Id)).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync((string name, Guid ownerId) => null);
           
            var handler = new UpdateRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object);

            //Act
            Repository result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateRepositoryForUser_ShouldThrowException_WhenUserAlreadyHasRepositoryWithSameName()
        {
            // Arrange
            var command = new UpdateRepositoryCommand(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test-repository", "test", false);

            User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null);
            RepositoryMember member = RepositoryMember.Create(foundUser, repository, RepositoryMemberRole.OWNER);
            _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.Find(repository.Id)).Returns(repository);
            _repositoryMemberRepositoryMock.Setup(x => x.FindRepositoryOwner(repository.Id)).ReturnsAsync(member);
            Repository repository2 = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a22d92a3"), "repository", "test", false, null);
            _repositoryRepositoryMock.Setup(x => x.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(repository2);

            var handler = new UpdateRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object);


            Func<Task> handle = async () =>
            {
                await handler.Handle(command, default);
            };

            //Assert
            await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());
        }
    }
}
