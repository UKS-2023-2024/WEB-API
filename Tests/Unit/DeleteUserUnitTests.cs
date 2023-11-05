using Application.Auth.Commands.Update;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Auth;
using Domain.Exceptions;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Auth.Commands.Delete;
using Domain.Auth.Exceptions;

namespace Tests.Unit
{
    public class DeleteUserUnitTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public DeleteUserUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            var user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), "test@gmail.com", "full name", "username", "password", UserRole.USER);
            _userRepositoryMock.Setup(x => x.FindUserById(user.Id)).ReturnsAsync(user);
        }

        [Fact]
        public async void Handle_ShouldReturnSuccess()
        {
            //Arrange
            var command = new DeleteUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"));

            var handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);

            //Act
            Func<Task> handle = async () =>
            {
                await handler.Handle(command, default);

            };
            //Assert
            await Should.NotThrowAsync(() => handle());
        }

        [Fact]
        public void Handle_ShouldThrowException_WhenIdInvalid()
        {
            //Arrange
            var command = new DeleteUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"));

            var handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);

            //Act
            Func<Task> handle = async () =>
            {
                await handler.Handle(command, default);

            };
            //Assert
            Should.ThrowAsync<UserNotFoundException>(() => handle()); ;
        }
    }
}
