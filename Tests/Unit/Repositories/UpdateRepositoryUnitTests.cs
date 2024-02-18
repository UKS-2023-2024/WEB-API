using Application.Repositories.Commands.Create;
using Domain.Auth.Enums;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;
using Application.Auth.Commands.Update;
using Application.Repositories.Commands.Update;
using Domain.Branches;
using FluentResults;
using Domain.Repositories.Exceptions;
using Domain.Shared.Interfaces;

namespace Tests.Unit.Repositories
{
    public class UpdateRepositoryUnitTests
    {
        private Mock<IRepositoryRepository> _repositoryRepositoryMock = new();
        private Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock = new();
        private Mock<IGitService> _gitServiceMock = new();

        [Fact]
        public async Task UpdateRepositoryForUser_ShouldBeSuccess_WhenRepositoryNameUnique()
        {
            // Arrange
            var command = new UpdateRepositoryCommand(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test-repository", "test", false);

            User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, foundUser);
            RepositoryMember member = RepositoryMember.Create(foundUser, repository, RepositoryMemberRole.OWNER);
            Branch branch1 = Branch.Create("name", new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), true, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
            repository.AddBranch(branch1);
            _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.Find(repository.Id)).Returns(repository);
            _repositoryMemberRepositoryMock.Setup(x => x.FindRepositoryOwner(repository.Id)).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync((string name, Guid ownerId) => null);
           
            var handler = new UpdateRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,_gitServiceMock.Object);

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
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, foundUser);
            RepositoryMember member = RepositoryMember.Create(foundUser, repository, RepositoryMemberRole.OWNER);
            _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.Find(repository.Id)).Returns(repository);
            _repositoryMemberRepositoryMock.Setup(x => x.FindRepositoryOwner(repository.Id)).ReturnsAsync(member);
            Repository repository2 = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a22d92a3"), "repository", "test", false, null, foundUser);
            _repositoryRepositoryMock.Setup(x => x.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(repository2);

            var handler = new UpdateRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,_gitServiceMock.Object);


            Func<Task> handle = async () =>
            {
                await handler.Handle(command, default);
            };

            //Assert
            await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());
        }
    }
}
