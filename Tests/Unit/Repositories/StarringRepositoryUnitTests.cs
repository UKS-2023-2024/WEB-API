using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class StarringRepositoryUnitTests
{
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly User _user;
    
    public StarringRepositoryUnitTests()
    {
        _user = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
        var repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null);
        var repository2 = Repository.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", true, null);
        var repository3 = Repository.Create(new Guid("8e9b1cc3-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", true, null);
        var repository4 = Repository.Create(new Guid("8e9b1cc4-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null);
        repository3.AddMember(RepositoryMember.Create(_user,repository3, RepositoryMemberRole.CONTRIBUTOR));
        repository4.AddToStarredBy(_user);
        _repositoryRepositoryMock = new Mock<IRepositoryRepository>();
        _repositoryMemberRepositoryMock = new Mock<IRepositoryMemberRepository>();
        _repositoryRepositoryMock.Setup(x => x.Find(repository1.Id)).Returns(repository1);
        _repositoryRepositoryMock.Setup(x => x.Find(repository2.Id)).Returns(repository2);
        _repositoryRepositoryMock.Setup(x => x.Find(repository3.Id)).Returns(repository3);
        _repositoryRepositoryMock.Setup(x => x.Find(repository4.Id)).Returns(repository4);

        var repositoryMember = RepositoryMember.Create(_user, repository3, RepositoryMemberRole.CONTRIBUTOR);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user.Id,repository3.Id)).ReturnsAsync(repositoryMember);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserDidntStarRepository()
    {
        //Arrange
        var command = new UnstarRepositoryCommand(_user,
            new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        var handler = new UnstarRepositoryCommandHandler(_repositoryRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotStarredException>(Handle);
    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenUserStarredRepositoryBefore()
    {
        //Arrange
        var command = new UnstarRepositoryCommand(_user,
            new Guid("8e9b1cc4-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        //Act
        var result = new UnstarRepositoryCommandHandler(_repositoryRepositoryMock.Object).Handle(command,default);
        
        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_Unstar_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var command = new UnstarRepositoryCommand(_user,
            new Guid("8e9b1cc5-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        var handler = new UnstarRepositoryCommandHandler(_repositoryRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenRepositoryPublicAndUserDidntStar()
    {
        //Arrange
        var command = new StarRepositoryCommand(_user,
            new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        //Act
        var result = new StarRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object).Handle(command,default);
        
        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryPublicAndUserDidStar()
    {
        //Arrange
        var command = new StarRepositoryCommand(_user,
            new Guid("8e9b1cc4-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        var handler = new StarRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryAlreadyStarredException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryPrivateAndUserNotMember()
    {
        //Arrange
        var command = new StarRepositoryCommand(_user,
            new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        var handler = new StarRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var command = new StarRepositoryCommand(_user,
            new Guid("8e9b1cc5-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        var handler = new StarRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenRepositoryPrivateAndUserMember()
    {
        //Arrange
        var command = new StarRepositoryCommand(_user,
            new Guid("8e9b1cc3-ffaa-4bf2-9f2c-5e00a21d92a9"));
        
        //Act
        var result = new StarRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object).Handle(command,default);
        
        //Assert
        result.IsFaulted.ShouldBe(false);
    }
}