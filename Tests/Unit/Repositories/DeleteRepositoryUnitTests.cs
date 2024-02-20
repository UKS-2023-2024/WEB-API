using Application.Organizations.Commands.Delete;
using Domain.Auth.Enums;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Repositories.Interfaces;
using Infrastructure.Auth.Repositories;
using Infrastructure.Organizations.Repositories;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Commands.Delete;
using MediatR;
using Domain.Repositories;
using Domain.Shared.Interfaces;

namespace Tests.Unit.Repositories
{
    public class DeleteRepositoryUnitTests
    { 
        private Mock<IRepositoryRepository> _repositoryRepositoryMock;
        private Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
        private Mock<IGitService> _gitServiceMock;
        private Mock<IUserRepository> _userRepositoryMock;

        public DeleteRepositoryUnitTests()
        {
            _repositoryRepositoryMock = new();
            _repositoryMemberRepositoryMock = new();
            _gitServiceMock = new();
            _userRepositoryMock = new();
        }

        [Fact]
        public async void DeleteOrganization_ShouldSucceed_WhenUserOwner()
        {
            var command = new DeleteRepositoryCommand(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"));

            User user = User.Create(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password",
                UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "test-repository", "test", false, null, user);
            RepositoryMember member = RepositoryMember.Create(user, repository, RepositoryMemberRole.OWNER);
            _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);
            _userRepositoryMock.Setup(x => x.FindUserById(user.Id)).ReturnsAsync(user);

            var handler = new DeleteRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
                _gitServiceMock.Object,_userRepositoryMock.Object);

            Func<Task> handle = async () =>
            {
                await handler.Handle(command, default);
            };

            //Assert
            await Should.NotThrowAsync(() => handle());
        }

        [Fact]
        public async void DeleteOrganization_ShouldFail_WhenUserNotaUTHORIZED()
        {
            var command = new DeleteRepositoryCommand(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"));

            User user = User.Create(new Guid("6e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
            Repository repository = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "test-repository", "test", false, null, user);
            RepositoryMember member = RepositoryMember.Create(user, repository, RepositoryMemberRole.READ);
            _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(member);
            _repositoryRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(repository);
            _userRepositoryMock.Setup(x => x.FindUserById(user.Id)).ReturnsAsync(user);
            
            var handler = new DeleteRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object,
                _gitServiceMock.Object,_userRepositoryMock.Object);

            Func<Task> handle = async () =>
            {
                await handler.Handle(command, default);
            };

            //Assert
            await Should.ThrowAsync<UnautorizedAccessException>(() => handle());
           
        }


    }
}
