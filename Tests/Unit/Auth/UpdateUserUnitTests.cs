using Application.Auth.Commands.Register;
using Domain.Auth.Enums;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Infrastructure.Auth.Services;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Auth.Commands.Update;
using Application.Auth.Queries.Login;
using Domain.Auth.Exceptions;

namespace Tests.Unit.Auth
{
    public class UpdateUserUnitTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISocialAccountRepository> _socialAccountRepositoryMock;

        public UpdateUserUnitTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _socialAccountRepositoryMock = new Mock<ISocialAccountRepository>();
            var user = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), "test@gmail.com", "full name", "username", "password", UserRole.USER);
            _userRepositoryMock.Setup(x => x.FindUserById(user.Id)).ReturnsAsync(user);
        }

        [Fact]
        public async void Handle_ShouldReturnSuccess()
        {
            //Arrange
            var command = new UpdateUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), "izmenjeno ime", "", "ftn", "Novi Sad", "", new List<SocialAccount>());

            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _socialAccountRepositoryMock.Object);

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
            var command = new UpdateUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "izmenjeno ime", "", "ftn", "Novi Sad", "", new List<SocialAccount>());

            var handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _socialAccountRepositoryMock.Object);

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
