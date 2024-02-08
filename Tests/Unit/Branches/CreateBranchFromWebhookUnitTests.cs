using Application.Branches.Commands.CreateFromWebhook;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class CreateBranchFromWebhookUnitTests
{
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IBranchRepository> _branchRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public CreateBranchFromWebhookUnitTests()
    {
        _repositoryRepositoryMock = new();
        _branchRepositoryMock = new();
        _userRepositoryMock = new();
    }

    [Fact]
    public async Task CreateBranch_ShouldPass_WhenDataIsCorrect()
    {
        // Arrange
        var username = "test";
        var repositoryName = "repo";
        var refName = "refs/heads/branch";
        var user = User.Create(new Guid(), "", "", "", "", UserRole.USER);
        var repository = Repository.Create("", "", false, null, user);
        var command = new CreateBranchFromWebhookCommand(username, repositoryName, refName);

        _userRepositoryMock.Setup(u => u.FindByUsername(It.IsAny<string>()))
            .ReturnsAsync(user)
            .Verifiable();
        _repositoryRepositoryMock.Setup(r => r.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync(repository)
            .Verifiable();
        _branchRepositoryMock.Setup(b => b.FindByNameAndRepositoryId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync((Branch?)null)
            .Verifiable();
        _branchRepositoryMock.Setup(b => b.Create(It.IsAny<Branch>()))
            .Verifiable();
        
        var handler = new CreateBranchFromWebhookCommandHandler(_branchRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _userRepositoryMock.Object);
        
        // Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        // Assert
        await Should.NotThrowAsync(handle);
        _branchRepositoryMock.Verify(r => r.Create(It.IsAny<Branch>()), Times.Once());
        _branchRepositoryMock.Verify(r => r.FindByNameAndRepositoryId(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once());
        _userRepositoryMock.Verify(r => r.FindByUsername(It.IsAny<string>()), Times.Once());
        _repositoryRepositoryMock.Verify(r => r.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once());
    }
    
    [Fact]
    public async Task CreateBranch_ShouldFail_WhenUserDoesntExist()
    {
        // Arrange
        var username = "test";
        var repositoryName = "repo";
        var refName = "refs/heads/branch";
        var user = User.Create(new Guid(), "", "", "", "", UserRole.USER);
        var repository = Repository.Create("", "", false, null, user);
        var command = new CreateBranchFromWebhookCommand(username, repositoryName, refName);

        _userRepositoryMock.Setup(u => u.FindByUsername(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        _repositoryRepositoryMock.Setup(r => r.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync(repository);
        _branchRepositoryMock.Setup(b => b.FindByNameAndRepositoryId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync((Branch?)null);
        _branchRepositoryMock.Setup(b => b.Create(It.IsAny<Branch>()))
            .Verifiable();
        
        var handler = new CreateBranchFromWebhookCommandHandler(_branchRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _userRepositoryMock.Object);
        
        // Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        // Assert
        await Should.ThrowAsync<UserNotFoundException>(handle);
    }
    
    [Fact]
    public async Task CreateBranch_ShouldFail_WhenRepositoryDoesntExist()
    {
        // Arrange
        var username = "test";
        var repositoryName = "repo";
        var refName = "refs/heads/branch";
        var user = User.Create(new Guid(), "", "", "", "", UserRole.USER);
        var repository = Repository.Create("", "", false, null, user);
        var command = new CreateBranchFromWebhookCommand(username, repositoryName, refName);

        _userRepositoryMock.Setup(u => u.FindByUsername(It.IsAny<string>()))
            .ReturnsAsync(user);
        _repositoryRepositoryMock.Setup(r => r.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync((Repository?)null);
        _branchRepositoryMock.Setup(b => b.FindByNameAndRepositoryId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync((Branch?)null);
        _branchRepositoryMock.Setup(b => b.Create(It.IsAny<Branch>()))
            .Verifiable();
        
        var handler = new CreateBranchFromWebhookCommandHandler(_branchRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _userRepositoryMock.Object);
        
        // Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        // Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(handle);
    }
    
    [Fact]
    public async Task CreateBranch_ShouldPass_WhenBranchAlreadyExists()
    {
        // Arrange
        var username = "test";
        var repositoryName = "repo";
        var refName = "refs/heads/branch";
        var user = User.Create(new Guid(), "", "", "", "", UserRole.USER);
        var repository = Repository.Create("", "", false, null, user);
        var branch = Branch.Create("branch", repository.Id, false, user.Id);
        branch = OverrideId<Branch>(branch, new Guid());
        var command = new CreateBranchFromWebhookCommand(username, repositoryName, refName);
        
        
        _userRepositoryMock.Setup(u => u.FindByUsername(It.IsAny<string>()))
            .ReturnsAsync(user);
        _repositoryRepositoryMock.Setup(r => r.FindByNameAndOwnerId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync(repository);
        _branchRepositoryMock.Setup(b => b.FindByNameAndRepositoryId(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync(branch);
        _branchRepositoryMock.Setup(b => b.Create(It.IsAny<Branch>()))
            .Verifiable();
        
        var handler = new CreateBranchFromWebhookCommandHandler(_branchRepositoryMock.Object,
            _repositoryRepositoryMock.Object, _userRepositoryMock.Object);
        
        // Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        // Assert
        _branchRepositoryMock.Verify(b => b.Create(It.IsAny<Branch>()), Times.Never);
    }
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
}