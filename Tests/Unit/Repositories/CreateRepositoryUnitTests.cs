using Application.Organizations.Commands.Create;
using Application.Repositories.Commands.Create;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Repositories.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using FluentResults;
using MediatR;
using Moq;
using Shouldly;
using Application.Repositories.Commands.Create.CreateForUser;
using Application.Repositories.Commands.Create.CreateForOrganization;
using Domain.Shared.Interfaces;

namespace Tests.Unit.Repositories;

public class CreateRepositoryUnitTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private Mock<IOrganizationRepository> _organizationRepositoryMock;
    private Mock<IGitService> _gitServiceMock;

    public CreateRepositoryUnitTests()
    {
        _userRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _organizationRepositoryMock = new();
        _gitServiceMock = new();
    }

    [Fact]
    public async Task CreateRepositoryForUser_ShouldBeSuccess_WhenRepositoryNameUnique()
    {
        // Arrange
        var command = new CreateRepositoryForUserCommand("test-repository", "test", false,
            new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"));

        User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
        _userRepositoryMock.Setup(x => x.FindUserById(foundUser.Id)).ReturnsAsync(foundUser);
        _repositoryRepositoryMock.Setup(x => x.FindByNameAndOwnerId("test-repository", foundUser.Id)).ReturnsAsync((string name, Guid ownerId) => null);
        _repositoryRepositoryMock.Setup(x => x.Create(It.IsAny<Repository>())).ReturnsAsync((Repository repository) => repository);

        var handler = new CreateRepositoryForUserCommandHandler(_userRepositoryMock.Object, _repositoryRepositoryMock.Object, _gitServiceMock.Object);

        //Act
        Guid repositoryId = await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryId.ShouldBeOfType<Guid>();
    }

    [Fact]
    async Task CreateRepositoryForOrganization_ShouldBeSuccess_WhenRepositoryNameUnique()
    {
        // Arrange
        var command = new CreateRepositoryForOrganizationCommand("test-repository", "test", false,
            new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a8"));

        User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
        _userRepositoryMock.Setup(x => x.FindUserById(foundUser.Id)).ReturnsAsync(foundUser);
        Organization organization = Organization.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a8"), "organizacija", "email@gmail.com", new(),foundUser);
        _organizationRepositoryMock.Setup(x => x.Find(organization.Id)).Returns(organization);
        _repositoryRepositoryMock.Setup(x => x.FindByNameAndOrganizationId("test-repository", organization.Id)).ReturnsAsync((string name, Guid organizationId) => null);
        _repositoryRepositoryMock.Setup(x => x.Create(It.IsAny<Repository>())).ReturnsAsync((Repository repository) => repository);

        var handler = new CreateRepositoryForOrganizationCommandHandler(_userRepositoryMock.Object, _repositoryRepositoryMock.Object, _organizationRepositoryMock.Object,_gitServiceMock.Object);

        //Act
        Guid repositoryId = await handler.Handle(command, CancellationToken.None);

        // Assert
        repositoryId.ShouldBeOfType<Guid>();
    }

    [Fact]
    async Task CreateRepositoryForUser_ShouldThrowException_WhenUserAlreadyHasRepositoryWithSameName()
    {
        //Arrange
        var command = new CreateRepositoryForUserCommand("test-repository", "test", false,
             new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"));

        User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
        _userRepositoryMock.Setup(x => x.FindUserById(foundUser.Id)).ReturnsAsync(foundUser);
        Repository repository = Repository.Create(new Guid("8e9b1cd0-35d3-4bf2-9f2c-5e00a21d92a8"), "test-repository", "test", false, null, foundUser);
        _repositoryRepositoryMock.Setup(x => x.FindByNameAndOwnerId("test-repository", foundUser.Id)).ReturnsAsync((string name, Guid ownerId) => repository);

        var handler = new CreateRepositoryForUserCommandHandler(_userRepositoryMock.Object, _repositoryRepositoryMock.Object,  _gitServiceMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());

    }

    [Fact]
    async Task CreateRepositoryForOrganization_ShouldThrowException_WhenOrganizationAlreadyHasRepositoryWithSameName()
    {
        // Arrange
        var command = new CreateRepositoryForOrganizationCommand("test-repository", "test", false,
            new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a8"));

        User foundUser = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
        _userRepositoryMock.Setup(x => x.FindUserById(foundUser.Id)).ReturnsAsync(foundUser);
        Organization organization = Organization.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d95a8"), "organizacija", "email@gmail.com", new(),foundUser);
        _organizationRepositoryMock.Setup(x => x.Find(organization.Id)).Returns(organization);
        Repository repository = Repository.Create(new Guid("8e9b1cd0-35d3-4bf2-9f2c-5e00a21d92a8"), "test-repository", "test", false, organization, foundUser);
        _repositoryRepositoryMock.Setup(x => x.FindByNameAndOrganizationId("test-repository", organization.Id)).ReturnsAsync((string name, Guid organizationId) => repository);
      
        var handler = new CreateRepositoryForOrganizationCommandHandler(_userRepositoryMock.Object, _repositoryRepositoryMock.Object, _organizationRepositoryMock.Object, _gitServiceMock.Object);

        //Act

        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());

    }

}